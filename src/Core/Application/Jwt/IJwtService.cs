using Domain.Entities.dbo.Users;
using SmartG.Api.ApplicationServices.Jwt;
using System.Threading.Tasks;

namespace Application.Jwt
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}