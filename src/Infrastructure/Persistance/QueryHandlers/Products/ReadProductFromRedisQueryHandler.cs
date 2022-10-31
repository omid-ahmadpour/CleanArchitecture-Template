using CleanTemplate.Application.Products.Query.ReadProductFromRedis;
using CleanTemplate.Domain.Entities.Products;
using CleanTemplate.Persistence.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PolyCache.Cache;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Persistence.QueryHandlers.Products
{
    public class ReadProductFromRedisQueryHandler : IRequestHandler<ReadProductFromRedisQuery, ReadProductFromRedisResponse>
    {
        private readonly CleanArchReadOnlyDbContext _dbContext;
        private readonly IStaticCacheManager _staticCacheManager;

        private const string _cachePrefix = "product_";
        private const int _cacheExpiryTime = 2; //minitues

        public ReadProductFromRedisQueryHandler(CleanArchReadOnlyDbContext dbContext,
                                                IStaticCacheManager staticCacheManager)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));
        }

        public async Task<ReadProductFromRedisResponse> Handle(ReadProductFromRedisQuery request, CancellationToken cancellationToken)
        {
            var productId = request.ProductId;

            var result = await _staticCacheManager.GetWithExpireTimeAsync(
                new CacheKey(_cachePrefix + productId),
                _cacheExpiryTime,
                async () => await GetProductAsync());

            return result;

            async Task<ReadProductFromRedisResponse> GetProductAsync()
            {
                var product = await _dbContext.Set<Product>().Where(a => a.Id == productId).Select(a =>
                       new ReadProductFromRedisResponse
                       {
                           Name = a.Name,
                           Price = a.Price
                       }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return product;
            }
        }
    }
}
