using CleanTemplate.Api.Controllers.v1.Products.Requests;
using CleanTemplate.ApiFramework.Tools;
using CleanTemplate.Application.Products.Command.AddProduct;
using CleanTemplate.Application.Products.Query.GetProductById;
using CleanTemplate.Application.Products.Query.GetProducts;
using CleanTemplate.Application.Products.Query.ReadProductFromRedis;
using CleanTemplate.Common.Utilities;
using CleanTemplate.Domain.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace CleanTemplate.Api.Controllers.v1.Products
{
    [ApiVersion("1")]
    public class ProductController : BaseControllerV1
    {
        [HttpPost]
        [SwaggerOperation("add a product")]
        public async Task<IActionResult> AddAsync([FromBody] AddProductRequest request)
        {
            var command = Mapper.Map<AddProductRequest, AddProductCommand>(request);

            var result = await Mediator.Send(command);

            return new ApiResult<int>(result);
        }

        [HttpGet]
        [SwaggerOperation("get a product by id")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int productId)
        {
            var result = await Mediator.Send(new GetProductByIdQuery { ProductId = productId });
            return new ApiResult<ProductQueryModel>(result);
        }

        [HttpGet("all")]
        [SwaggerOperation("get all products")]
        public async Task<IActionResult> GetAllAsync(GetProductsRequest request)
        {
            var query = Mapper.Map<GetProductsQuery>(request);
            var result = await Mediator.Send(query);
            return new ApiResult<PagedResult<Product>>(result);
        }

        [HttpGet("cache-redis")]
        [SwaggerOperation("get a product from cache. this is a example for how to use cache")]
        public async Task<IActionResult> ReadFromCacheAsync([FromQuery] int productId)
        {
            var result = await Mediator.Send(new ReadProductFromRedisQuery(productId));
            return new ApiResult<ReadProductFromRedisResponse>(result);
        }
    }
}