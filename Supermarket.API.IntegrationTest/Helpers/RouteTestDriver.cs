using System;
using System.Net;
using System.Net.Http;

namespace Supermarket.API.IntegrationTest
{
    public class RouteTestDriver
    {
        private string _localHostBaseUrl;
        private readonly HttpClient Client;

        public RouteTestDriver(HttpClient client)
        {
            Client = client;
            _localHostBaseUrl = client.BaseAddress.OriginalString;
        }

        public bool UrlReturnsSuccessStatusCode(string relativeUrlForTest)
        {
            var httpCallResult = TestUrl(relativeUrlForTest);
            return httpCallResult.IsSuccessStatusCode;
        }

        public bool UrlReturns404NotFoundStatusCode(string relativeUrlForTest)
        {
            var httpCallResult = TestUrl(relativeUrlForTest);
            return httpCallResult.StatusCode == HttpStatusCode.NotFound;
        }

        public bool UrlReturns400BadRequestStatusCode(string relativeUrlForTest)
        {
            var httpCallResult = TestUrl(relativeUrlForTest);
            return httpCallResult.StatusCode == HttpStatusCode.BadRequest;
        }

        private HttpResponseMessage TestUrl(string relativeUrlForTest)
        {
            var uriToTest = new Uri(_localHostBaseUrl + (relativeUrlForTest ?? string.Empty));
            var httpCallResult = Client.GetAsync(uriToTest).Result;
            return httpCallResult;
        }
    }
    
}
