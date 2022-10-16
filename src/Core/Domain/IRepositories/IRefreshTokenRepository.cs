using CleanTemplate.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Domain.IRepositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task UpsertRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task<bool> ValidateRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken);
    }
}
