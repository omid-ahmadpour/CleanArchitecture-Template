using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Dispatching
{
    public interface IDispatcher
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
