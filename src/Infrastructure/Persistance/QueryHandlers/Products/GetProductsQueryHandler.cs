using CleanTemplate.Application.Products.Query.GetProducts;
using CleanTemplate.Common.Utilities;
using CleanTemplate.Domain.Entities.Products;
using CleanTemplate.Persistence.Db;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Persistence.QueryHandlers.Products
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<Product>>
    {
        private readonly CleanArchReadOnlyDbContext _dbContext;

        public GetProductsQueryHandler(CleanArchReadOnlyDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<PagedResult<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Set<Product>().GetPaged(request.Page, request.PageSize);

            return products;
        }
    }
}
