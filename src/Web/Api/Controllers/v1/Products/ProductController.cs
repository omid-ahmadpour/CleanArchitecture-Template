using CleanTemplate.Api.Controllers.v1.Products.Requests;
using CleanTemplate.ApiFramework.Tools;
using CleanTemplate.Application.Products.Command.AddProduct;
using CleanTemplate.Application.Products.Query.GetProductById;
using CleanTemplate.Application.Products.Query.ReadProductFromRedis;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace CleanTemplate.Api.Controllers.v1.Products
{
    [ApiVersion("1")]
    public class ProductController : BaseControllerV1
    {
        [HttpGet]
        [SwaggerOperation("get a product by id")]
        public async Task<ApiResult<ProductQueryModel>> GetByIdAsync([FromQuery] int productId)
        {
            var result = await Mediator.Send(new GetProductByIdQuery() { ProductId = productId });
            return new ApiResult<ProductQueryModel>(result);
        }

        [HttpPost]
        [SwaggerOperation("add a product")]
        public async Task<ApiResult<int>> AddAsync([FromBody] AddProductRequest request)
        {
            var command = Mapper.Map<AddProductRequest, AddProductCommand>(request);

            var result = await Mediator.Send(command);

            return new ApiResult<int>(result);
        }

        [HttpGet("cache-redis")]
        [SwaggerOperation("get a product from cache. this is a example for how to use cache")]
        public async Task<ApiResult<ReadProductFromRedisResponse>> ReadFromCacheAsync([FromQuery] int productId)
        {
            var result = await Mediator.Send(new ReadProductFromRedisQuery(productId));
            return new ApiResult<ReadProductFromRedisResponse>(result);
        }
    }
}