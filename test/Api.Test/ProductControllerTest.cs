using AutoMapper;
using CleanTemplate.Api.Controllers.v1.Products;
using CleanTemplate.ApiFramework.Tools;
using CleanTemplate.Application.Products.Query.GetProductById;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CleanTemplate.Api.Test
{
    public class ProductControllerTest
    {
        ProductController productController;

        public ProductControllerTest()
        {
            productController = new ProductController();
        }

        [Theory]
        [InlineData(1, 20)]
        public async Task GetByIdAsyncTest(int id1, int id2)
        {
            //arrange
            var validId = id1;
            var invalidId = id2;

            //act
            var result = await productController.GetByIdAsync(validId);
            var nullResult = await productController.GetByIdAsync(invalidId);

            //assert
            Assert.IsType<ApiResult<ProductQueryModel>>(result);

            var nullItem = nullResult.Data;
            Assert.Null(nullItem);

            //var item = result.Data as ProductQueryModel;
            //Assert.IsType<ProductQueryModel>(item);

            //var productItem = item as ProductQueryModel;
            //Assert.Equal(validId, productItem.ProductId);
        }
    }
}
