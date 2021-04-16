using MediatR;

namespace Application.Products.Query.ReadProductFromRedis
{
    public class ReadProductFromRedisQuery : IRequest<ReadProductFromRedisResponse>
    {
        public ReadProductFromRedisQuery(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; private set; }
    }
}
