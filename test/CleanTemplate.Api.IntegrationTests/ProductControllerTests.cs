using CleanTemplate.Api.Controllers.v1.Products.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleanTemplate.Api.IntegrationTests
{
    public class ProductControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProductControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/v1/Product?productId=1")]
        public async Task GetByIdEndpoint_Should_ReturnOk(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/api/v1/Product/all?Page=1&PageSize=1")]
        public async Task GetAllEndpoint_Should_ReturnOk(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory, ClassData(typeof(AddProductTestData))]
        public async Task AddEndpoint_Should_ReturnOk(string name, decimal price)
        {
            // Arrange
            var client = _factory.CreateClient();
            const string url = "/api/v1/Product";
            var request = new AddProductRequest { Name = name, Price = price };
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(url, data);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        public class AddProductTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "sample product name", 1000 };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
