using Domain.Entities.Users;
using System.Threading.Tasks;

namespace Persistance.Jwt
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}
