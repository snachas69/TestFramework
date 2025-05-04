using RestSharp;
using Serilog;
using System.Net;
using TestProject1.Core.Base;

namespace TestProject1.Tests
{
    [TestFixture, Category("API")]
    [Parallelizable(ParallelScope.All)]
    public class ApiClientTests : TestBaseApi
    {
        [Test]
        public async Task GetUsers_ShouldReturnUserListWithAllFields()
        {
            var response = await (base.ApiClientPage?.GetResponse("/users")
                ?? throw new NullReferenceException("API client is NULL"));

            base.ApiClientPage.ValidateStatusCode(response.StatusCode, HttpStatusCode.OK);

            var users = base.ApiClientPage.ConvertToUserJArray(response?.Content ?? string.Empty);

            base.ApiClientPage.ValidateAllFieldsArePresentInAllUsers(users);
        }

        [Test]
        public async Task GetUsers_ShouldContainProperContentTypeHeader()
        {
            var response = await (base.ApiClientPage?.GetResponse("/users")
                ?? throw new NullReferenceException("API client is NULL"));

            Log.Information("Received content-type: {ContentType}", response?.ContentType);

            base.ApiClientPage.ValidateContentTypeHeaderExists(response?.ContentType);

            base.ApiClientPage.ValidateContentTypeHeaderStartsWith(response?.ContentType, "application/json");

            base.ApiClientPage.ValidateStatusCode(response?.StatusCode 
                ?? throw new NullReferenceException("Status code is NULL"), 
                HttpStatusCode.OK);
        }

        [Test]
        public async Task GetUsers_ShouldContain10UniqueUsersWithValidData()
        {
            var response = await (base.ApiClientPage?.GetResponse("/users")
                ?? throw new NullReferenceException("API client is NULL"));

            var users = base.ApiClientPage.ConvertToUserJArray(response?.Content ?? string.Empty);

            base.ApiClientPage.ValidateStatusCode(response?.StatusCode
                ?? throw new NullReferenceException("Status code is NULL"), 
                HttpStatusCode.OK);

            base.ApiClientPage.ValidateTheNumberOfElelmentsInResponse(users.Count, 10);

            base.ApiClientPage.ValidateUserDataIsCorrect(users);
        }

        [Test]
        [NonParallelizable]
        public async Task CreateUser_ShouldReturnCreatedUserWithId()
        {
            var response = await (base.ApiClientPage?.PostResponse("/users", 
                new { name = "Test Name", username = "TestUser" })
                ?? throw new NullReferenceException("API client is NULL"));

            Log.Information("Received response: {StatusCode}, Content: {Content}", response.StatusCode, response.Content);

            base.ApiClientPage.ValidateStatusCode(response?.StatusCode
                ?? throw new NullReferenceException("Status code is NULL"), 
                HttpStatusCode.Created);

            var responseData = base.ApiClientPage.ConvertToUserJObject(response?.Content ?? string.Empty);

            base.ApiClientPage.ValidateResponseContainsFiled(responseData, "id");
        }

        [Test]
        [NonParallelizable]
        public async Task GetInvalidEndpoint_ShouldReturnNotFound()
        {
            var response = await (base.ApiClientPage?.GetResponse("/invalidendpoint")
                ?? throw new NullReferenceException("API client is NULL"));

            base.ApiClientPage.ValidateResponseStatus(response.ResponseStatus, ResponseStatus.Completed);

            base.ApiClientPage.ValidateStatusCode(response.StatusCode, HttpStatusCode.NotFound);
        }
    }
}
