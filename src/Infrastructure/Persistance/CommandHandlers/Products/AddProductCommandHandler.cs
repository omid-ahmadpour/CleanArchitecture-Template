using CleanTemplate.Application.Products.Command.AddProduct;
using CleanTemplate.Common.Exceptions;
using CleanTemplate.Domain.Entities.Products;
using CleanTemplate.Persistance.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Persistance.CommandHandlers.Products
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
    {
        private readonly CleanArchWriteDbContext dbContext;
        private readonly ILogger<AddProductCommandHandler> logger;

        public AddProductCommandHandler(CleanArchWriteDbContext dbContext, ILogger<AddProductCommandHandler> logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await dbContext.Set<Product>().FirstOrDefaultAsync(a => a.Name == request.Name);

            if (existingProduct != null)
            {
                throw new ExistingRecordException("This Product has been added");
            }

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };

            await dbContext.Set<Product>().AddAsync(product, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Product Inserted", product);

            return product.Id;
        }
    }
}
