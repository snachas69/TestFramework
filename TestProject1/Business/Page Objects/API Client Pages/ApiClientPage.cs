using Newtonsoft.Json.Linq;
using RestSharp;
using Serilog;
using System.Net;

namespace TestProject1.Business.PageObjects.ApiClientPages
{
    public class ApiClientPage
    {
        private RestClient? Client { get; set; }

        public ApiClientPage(RestClient? client)
        {
            this.Client = client;

            Log.Information("API client page has been created");
        }

        //Private methods

        private async Task<RestResponse> ExecuteRequest(RestRequest? request)
        {
            Log.Information($"Sending {request?.Method} request to {request?.Resource}");

            var response = await (this.Client?.ExecuteAsync(request 
                ?? throw new ArgumentNullException("The REST request cannot return null"))
                ?? throw new NullReferenceException("API client is NULL"));

            return response;
        }

        //Public methods

        public JArray ConvertToUserJArray(string? content)
        {
            return JArray.Parse(content ?? string.Empty);
        }

        public JObject ConvertToUserJObject(string? content)
        {
            return JObject.Parse(content ?? string.Empty);
        }

        public async Task<RestResponse> GetResponse(string? resource)
        {
            var request = new RestRequest(resource, Method.Get);

            return await ExecuteRequest(request);
        }

        public async Task<RestResponse> PostResponse(string? resource, object obj)
        {
            var request = new RestRequest(resource, Method.Post)
                .AddJsonBody(obj);

            return await ExecuteRequest(request);
        }

        //Validate

        public void ValidateAllFieldsArePresentInAllUsers(JArray? users)
        {
            Log.Information("Validating response content structure");

            //Assert
            foreach (var user in users ?? new JArray())
            {
                Assert.That(user["id"], Is.Not.Null, 
                    "\"id\" field is abscent or empty");
                Assert.That(user["name"], Is.Not.Null,
                    "\"name\" field is abscent or empty");
                Assert.That(user["username"], Is.Not.Null,
                    "\"username\" field is abscent or empty");
                Assert.That(user["email"], Is.Not.Null,
                    "\"email\" field is abscent or empty");
                Assert.That(user["address"], Is.Not.Null,
                    "\"address\" field is abscent or empty");
                Assert.That(user["phone"], Is.Not.Null,
                    "\"phone\" field is abscent or empty");
                Assert.That(user["website"], Is.Not.Null,
                    "\"website\" field is abscent or empty");
                Assert.That(user["company"], Is.Not.Null,
                    "\"company\" field is abscent or empty");
            }
        }

        public void ValidateContentTypeHeaderExists(string? header)
        {
            //Assert
            Assert.That(header, Is.Not.Null.And.Not.Empty, "Content-Type header is missing or empty.");
        }

        public void ValidateContentTypeHeaderStartsWith(string? header, string? resource)
        {
            //Assert
            Assert.That(header?.StartsWith(resource ?? string.Empty) ?? false,
                $"Unexpected content type: {header}");
        }

        public void ValidateStatusCode(HttpStatusCode actualStatusCode, HttpStatusCode expectedStatusCode)
        {
            Log.Information($"Received response with status code: {actualStatusCode}");

            //Assert
            Assert.That(actualStatusCode, Is.EqualTo(expectedStatusCode));
        }

        public void ValidateResponseStatus(ResponseStatus actualResponseStatus, ResponseStatus expectedResponseStatus)
        {
            //Assert
            Assert.That(actualResponseStatus, Is.EqualTo(expectedResponseStatus),
                "Request did not complete successfully. Check network or endpoint.");

            Log.Information($"Response has been completed");
        }

        public void ValidateTheNumberOfElelmentsInResponse(int actualNumber, int expectedNumber)
        {
            //Assert
            Assert.That(actualNumber, Is.EqualTo(expectedNumber));
        }

        public void ValidateUserDataIsCorrect(JArray? users)
        {
            //Arrange
            var ids = new HashSet<int?>();

            //Act
            foreach (var user in users ?? new JArray())
            {
                var id = user["id"]?.Value<int>();

                //Assert
                Assert.That(ids.Add(id), Is.True, $"Duplicate ID found: {id}");
                Assert.That(user["name"]?.ToString(), Is.Not.Empty);
                Assert.That(user["username"]?.ToString(), Is.Not.Empty);
                Assert.That(user["company"]?.Value<JObject>()?["name"]?.ToString(), Is.Not.Empty);
            }
        }

        public void ValidateResponseContainsFiled(JObject? responseData, string? field)
        {
            //Assert
            Assert.That(responseData?[field ?? string.Empty], Is.Not.Null, 
                $"Response does not contain an {field ?? string.Empty} field.");
        }
    }
}
