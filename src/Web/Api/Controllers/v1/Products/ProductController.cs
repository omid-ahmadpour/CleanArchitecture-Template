using Api.Controllers.v1.Products.Requests;
using ApiFramework.Tools;
using Application.Products.Command.AddProduct;
using Application.Products.Query.GetProductById;
using Application.Products.Query.ReadProductFromRedis;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Api.Controllers.v1.Products
{
    [ApiVersion("1")]
    public class ProductController : BaseControllerV1
    {
        public ProductController(ILogger<ProductController> logger,
                                 IMediator mediator,
                                 IMapper mapper)
            : base(logger, mediator, mapper)
        {}

        [HttpGet]
        [SwaggerOperation("get a product by id")]
        public async Task<ApiResult<ProductQueryModel>> GetByIdAsync([FromQuery] int productId)
        {
            var result = await _mediator.Send(new GetProductByIdQuery() { ProductId = productId });
            return new ApiResult<ProductQueryModel>(result);
        }

        [HttpPost]
        [SwaggerOperation("add a product")]
        public async Task<ApiResult<int>> AddAsync(AddProductRequest request)
        {
            var command = _mapper.Map<AddProductRequest, AddProductCommand>(request);

            var result = await _mediator.Send(command);

            return new ApiResult<int>(result);
        }

        [HttpGet("cache-redis")]
        [SwaggerOperation("get a product from cache. this is a example for how to use cache")]
        public async Task<ApiResult<ReadProductFromRedisResponse>> ReadFromCacheAsync([FromQuery] int productId)
        {
            var result = await _mediator.Send(new ReadProductFromRedisQuery(productId));
            return new ApiResult<ReadProductFromRedisResponse>(result);
        }
    }
}