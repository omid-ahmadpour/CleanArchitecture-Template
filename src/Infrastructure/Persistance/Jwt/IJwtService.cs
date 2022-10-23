using CleanTemplate.Domain.Entities.Users;
using System.Threading.Tasks;

namespace CleanTemplate.Persistance.Jwt
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
        int? ValidateJwtAccessTokenAsync(string token);
    }
}
