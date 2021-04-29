using Api.Controllers.v1.Products.Requests;
using ApiFramework.Tools;
using Application.Products.Command.AddProduct;
using Application.Products.Query.GetProductById;
using Application.Products.Query.ReadProductFromRedis;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Api.Controllers.v1.Products
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class ProductController : BaseController
    {
        public ProductController(ILogger<ProductController> logger,
                                 IMediator mediator)
            : base(logger, mediator)
        {}

        [HttpGet]
        public async Task<ApiResult<ProductQueryModel>> GetByIdAsync([FromQuery] int productId)
        {
            var result = await Mediator.Send(new GetProductByIdQuery() { ProductId = productId });
            return new ApiResult<ProductQueryModel>(result);
        }

        [HttpPost]
        public async Task<ApiResult<int>> AddAsync(AddProductRequest request)
        {
            var command = new AddProductCommand
            {
                Name = request.Name,
                Price = request.Price
            };

            var result = await Mediator.Send(command);

            return new ApiResult<int>(result);
        }

        [HttpGet("cache-redis")]
        public async Task<ApiResult<ReadProductFromRedisResponse>> ReadFromCacheAsync([FromQuery] int productId)
        {
            var result = await Mediator.Send(new ReadProductFromRedisQuery(productId));
            return new ApiResult<ReadProductFromRedisResponse>(result);
        }
    }
}