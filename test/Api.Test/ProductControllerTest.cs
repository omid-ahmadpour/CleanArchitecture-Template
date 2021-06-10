using Api.Controllers.v1.Products;
using ApiFramework.Tools;
using Application.Products.Query.GetProductById;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Api.Test
{
    public class ProductControllerTest
    {
        ProductController productController;
        IMediator mediator;
        ILogger<ProductController> logger;
        IMapper mapper;

        public ProductControllerTest()
        {
            mediator = new Mock<IMediator>().Object;
            logger = new Mock<ILogger<ProductController>>().Object;
            mapper = new Mock<IMapper>().Object;
            productController = new ProductController(logger, mediator,mapper);
        }

        [Theory]
        [InlineData(1,20)]
        public async Task GetByIdAsyncTest(int id1,int id2)
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
