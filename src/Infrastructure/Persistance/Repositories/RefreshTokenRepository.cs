using CleanTemplate.Common;
using CleanTemplate.Domain.Entities.Users;
using CleanTemplate.Domain.IRepositories;
using CleanTemplate.Persistance.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Persistance.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository, IScopedDependency
    {
        public RefreshTokenRepository(CleanArchWriteDbContext dbContext)
           : base(dbContext)
        {
        }
        public async Task UpsertRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            var token = await Table.Where(x => x.UserId == refreshToken.UserId).AsNoTracking().SingleOrDefaultAsync(cancellationToken);
            if (token == null)
            {
                refreshToken.Created = DateTime.Now;
                await base.AddAsync(refreshToken, cancellationToken);
            }
            else
            {
                refreshToken.Created = token.Created;
                refreshToken.Id = token.Id;
                await base.UpdateAsync(refreshToken, cancellationToken);
            }
        }
        public async Task<bool> ValidateRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            var result = await Table.Where(x => x.UserId == refreshToken.UserId).AsNoTracking().SingleOrDefaultAsync(cancellationToken);
            if (result.Token != refreshToken.Token)
                throw new CleanArchAppException("RefreshToken is not valid");
            return true;
        }
    }
}
