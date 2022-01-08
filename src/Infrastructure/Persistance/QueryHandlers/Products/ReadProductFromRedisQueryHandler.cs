using CleanTemplate.Application.Products.Query.ReadProductFromRedis;
using CleanTemplate.Domain.Entities.Products;
using CleanTemplate.Persistance.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PolyCache.Cache;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Persistance.QueryHandlers.Products
{
    public class ReadProductFromRedisQueryHandler : IRequestHandler<ReadProductFromRedisQuery, ReadProductFromRedisResponse>
    {
        private readonly CleanArchReadOnlyDbContext dbContext;
        private readonly IStaticCacheManager staticCacheManager;
        private const string CachePrefix = "product_";
        private const int CacheExpiryTime = 2; //minitues

        public ReadProductFromRedisQueryHandler(CleanArchReadOnlyDbContext dbContext,
                                                IStaticCacheManager staticCacheManager)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));
        }

        public async Task<ReadProductFromRedisResponse> Handle(ReadProductFromRedisQuery request, CancellationToken cancellationToken)
        {
            var productId = request.ProductId;

            var result = await staticCacheManager.GetWithExpireTimeAsync(
                new CacheKey(CachePrefix + productId),
                CacheExpiryTime,
                async () => await GetProductAsync());

            return result;

            async Task<ReadProductFromRedisResponse> GetProductAsync()
            {
                var product = await dbContext.Set<Product>().Where(a => a.Id == productId).Select(a =>
                       new ReadProductFromRedisResponse
                       {
                           Name = a.Name,
                           Price = a.Price
                       }).FirstOrDefaultAsync();

                return product;
            }
        }
    }
}
