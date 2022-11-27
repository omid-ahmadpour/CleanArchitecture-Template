using CleanTemplate.Api.Controllers.v1.Products.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleanTemplate.Api.IntegrationTest
{
    public class ProductControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProductControllerTest(WebApplicationFactory<Program> factory)
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
    }
}
