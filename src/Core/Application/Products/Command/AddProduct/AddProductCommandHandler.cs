using Application.Common.Exceptions;
using Domain.Entities.dbo.Products;
using MediatR;
using Persistance.Db;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Command.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly IAppDbContext dbContext;

        public AddProductCommandHandler(IAppDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = dbContext.Set<Product>().Where(a =>a.Name.Contains(request.Name,StringComparison.OrdinalIgnoreCase));

            if (!existingProduct.Any())
            {
                throw new ExistingRecordException("This Product has been added");
            }

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };

            await dbContext.Set<Product>().AddAsync(product);
            return product.Id;
        }
    }
}
