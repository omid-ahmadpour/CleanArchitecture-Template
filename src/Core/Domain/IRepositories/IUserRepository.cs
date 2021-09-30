using Domain.Entities.Users;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);

        Task AddAsync(User user, string password, CancellationToken cancellationToken);

        Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken);

        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    }
}