using Application.Common.Exceptions;
using Domain.Entities.dbo.Products;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AddProductCommandHandler> logger;

        public AddProductCommandHandler(IAppDbContext dbContext, ILogger<AddProductCommandHandler> logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

            logger.LogInformation("Product Inserted", product);

            return product.Id;
        }
    }
}
