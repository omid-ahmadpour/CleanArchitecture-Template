using CleanTemplate.Application.Products.Query.GetProductById;
using CleanTemplate.Domain.Entities.Products;
using CleanTemplate.Persistance.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Persistance.QueryHandlers.Products
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductQueryModel>
    {
        private readonly CleanArchReadOnlyDbContext _dbContext;

        public GetProductByIdQueryHandler(CleanArchReadOnlyDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ProductQueryModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var existingProduct = await _dbContext.Set<Product>().Where(a => a.Id == request.ProductId).Select(a =>
               new ProductQueryModel
               {
                   Name = a.Name,
                   Price = a.Price
               }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return existingProduct;
        }
    }
}