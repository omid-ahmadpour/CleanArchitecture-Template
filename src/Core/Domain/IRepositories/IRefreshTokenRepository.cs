using CleanTemplate.Domain.Entities.Users;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Domain.IRepositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task AddOrUpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    }
}
