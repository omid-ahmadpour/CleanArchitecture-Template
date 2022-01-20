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
        private readonly CleanArchWriteDbContext _dbContext;
        private readonly ILogger<AddProductCommandHandler> _logger;

        public AddProductCommandHandler(CleanArchWriteDbContext dbContext, ILogger<AddProductCommandHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            var existingProduct = await _dbContext.Set<Product>().FirstOrDefaultAsync(a => a.Name == request.Name, cancellationToken);

            if (existingProduct != null)
            {
                throw new ExistingRecordException("This Product has been added");
            }

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };

            await _dbContext.Set<Product>().AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product Inserted", product);

            return product.Id;
        }
    }
}
