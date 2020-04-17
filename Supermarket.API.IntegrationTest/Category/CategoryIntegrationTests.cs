using Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Xunit;

namespace Supermarket.API.IntegrationTest
{
    //[AutoRollback]
    public class CategoryIntegrationTests : IClassFixture<AppTestFixture> // AppTestFixture<TestStartup> //<WebApplicationFactory<Startup>>
    {
        readonly AppTestFixture _fixture;
        readonly HttpClient Client;

        //private readonly WebApplicationFactory<Startup> _fixture;

        public CategoryIntegrationTests(AppTestFixture fixture)
        {
            _fixture = fixture;
            fixture.Server.PreserveExecutionContext = true;
            //Client.pre
            Client = fixture.CreateClient();
            // _client = _fixture.ClientOptions;

        }

        [Fact]
        public async void GetCategoriesIntegrationTest()
        {

            // The endpoint or route of the controller action.
            var httpResponse = await Client.GetAsync("/api/categories");

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(stringResponse);
            Assert.Contains(categories, p => p.Name == "Fruit");
        }

        [Fact]
        public async void AddCategoryIntegrationTest()
        {
            // The endpoint or route of the controller action.
            var request = new
            {
                Url = "/api/categories",
                Body = new
                {
                    Name = "Drinks",
                }
            };

            var context = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json");

            // Act
            var httpResponse = await Client.PostAsync(request.Url, context);

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var category = JsonConvert.DeserializeObject<Category>(stringResponse);
            Assert.Equal("Drinks", category.Name);
        }

    }
}
