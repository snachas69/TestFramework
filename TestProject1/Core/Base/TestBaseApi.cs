using RestSharp;
using TestProject1.Business.PageObjects.ApiClientPages;

namespace TestProject1.Core.Base
{
    public abstract class TestBaseApi : TestBase
    {
        protected ApiClientPage? ApiClientPage;
        private RestClient _client;
        private const string BaseUrl = "https://jsonplaceholder.typicode.com";

        [SetUp]
        public void Setup()
        {
            var options = new RestClientOptions(BaseUrl)
            {
                ThrowOnAnyError = false
            };

            this._client = new RestClient(options);
            this.ApiClientPage = new ApiClientPage(_client);
        }

        [TearDown]
        public void TearDown()
        {
            this._client?.Dispose();
        }
    }
}
