using Domain.Entities.dbo.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Db;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Query.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductQueryModel>
    {
        private readonly IAppDbContext dbContext;

        public GetProductByIdQueryHandler(IAppDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ProductQueryModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var existingProduct = await dbContext.Set<Product>().Where(a => a.Id == request.ProductId).Select( a =>
                new ProductQueryModel
                {
                    Name = a.Name,
                    Price = a.Price
                }).FirstOrDefaultAsync();

            return existingProduct;
        }
    }
}