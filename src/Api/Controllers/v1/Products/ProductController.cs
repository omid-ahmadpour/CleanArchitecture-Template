using Api.Controllers.v1.Products.Requests;
using Api.Tools;
using Application.Products.Command.AddProduct;
using Application.Products.Query.GetProductById;
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
        {

        }

        [HttpGet("product")]
        public async Task<ApiResult<ProductQueryModel>> GetByIdAsync([FromRoute] int productId)
        {
            var result = await Mediator.Send(new GetProductByIdQuery() { ProductId = productId });
            return new ApiResult<ProductQueryModel>(result);
        }

        [HttpPost("product")]
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
    }
}