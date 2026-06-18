using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Dispatching
{
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

    public interface IRequestPipelineBehavior<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
    }
}
