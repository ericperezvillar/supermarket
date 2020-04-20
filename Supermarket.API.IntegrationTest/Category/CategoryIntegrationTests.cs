using Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Xunit;
using Domain.Repositories;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Supermarket.API.IntegrationTest
{
    // This AutoRollback attribute will not insert, update or delete the database. It will manage the Tests 
    // through transactions that will rollback at the end of every test. 
    // If we remove this attribute, data will be inserted/updated/deleted on the database
    [AutoRollback]
    public class CategoryIntegrationTests : IClassFixture<AppTestFixture>, IDisposable
    {
        #region Private Properties
        private readonly AppTestFixture _fixture;
        private readonly HttpClient Client;
        private RouteTestDriver _testDriver;
        private readonly CategoryRepository _categoryRepository;
        #endregion

        public CategoryIntegrationTests(AppTestFixture fixture)
        {
            _fixture = fixture;
            fixture.Server.PreserveExecutionContext = true;
            Client = fixture.CreateClient();
            _testDriver = new RouteTestDriver(Client);
            _categoryRepository = (CategoryRepository)fixture.Services.GetService(typeof(ICategoryRepository));
        }

        public void Dispose()
        {
            _testDriver = null;
        }

        #region Tests
        [Theory]
        [InlineData("/api/categories")]
        [InlineData("/api/products")]
        public void TestUrlResults(string url)
        {
            Assert.True(_testDriver.UrlReturnsSuccessStatusCode(url));
        }

        [Theory]
        [InlineData("/api/categoriesWrong")]
        [InlineData("/api/productsWrong")]
        public void TestUrlResultsNotFound(string url)
        {
            Assert.True(_testDriver.UrlReturns404NotFoundStatusCode(url));
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
            var listOfCategoriesBefore = _categoryRepository.ListAsync().Result.Count();

            var categoryName = "Drinks";
            var category = await PostCategory(categoryName);

            Assert.Equal(categoryName, category.Name);

            var listOfCategoriesAfter = _categoryRepository.ListAsync().Result.Count();
            Assert.Equal(listOfCategoriesBefore + 1, listOfCategoriesAfter);
        }

        [Fact]
        public async void UpdateCategoryIntegrationTest()
        {
            var categoryName = "Drinks";
            var categoryPost = await PostCategory(categoryName);

            Assert.Equal(categoryName, categoryPost.Name);

            var getInsertedCategory = _categoryRepository.FindCategoryByName(categoryName);
            // The endpoint or route of the controller action.
            var request = new
            {
                Url = string.Format("/api/categories/{0}", getInsertedCategory.Id),
                Body = new
                {
                    name = "Beauty",
                }
            };

            var context = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.Default, "application/json");

            // Act
            var httpResponse = await Client.PutAsync(request.Url, context);

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var categoryUpdate = JsonConvert.DeserializeObject<Category>(stringResponse);
            Assert.Equal("Beauty", categoryUpdate.Name);
        }

        [Fact]
        public async void DeleteCategoryIntegrationTest()
        {
            var categoryName = "Drinks";
            var categoryPost = await PostCategory(categoryName);

            Assert.Equal(categoryName, categoryPost.Name);

            // The endpoint or route of the controller action.
            var request = new
            {
                Url = string.Format("/api/categories/{0}", categoryPost.Id)
            };

            // Act
            var httpResponse = await Client.DeleteAsync(request.Url);

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var categoryDelete = JsonConvert.DeserializeObject<Category>(stringResponse);
            Assert.Equal(categoryPost.Name, categoryDelete.Name);
        }
        #endregion

        #region PostCategory
        private async Task<Category> PostCategory(string categoryName)
        {
            // The endpoint or route of the controller action.
            var request = new
            {
                Url = "/api/categories",
                Body = new
                {
                    name = "Drinks",
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

            return category;
        }
        #endregion

    }
}
