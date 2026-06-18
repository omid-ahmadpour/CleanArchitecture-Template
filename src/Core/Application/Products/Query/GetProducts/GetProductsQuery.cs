using CleanTemplate.Common.Utilities;
using CleanTemplate.Domain.Entities.Products;
using CleanTemplate.Dispatching;

namespace CleanTemplate.Application.Products.Query.GetProducts
{
    public class GetProductsQuery : IRequest<PagedResult<Product>>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
