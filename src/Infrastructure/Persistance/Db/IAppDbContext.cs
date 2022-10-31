namespace CleanTemplate.Persistence.Db
{
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IAppDbContext
    {
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellation);
        Task<int> ExecuteSqlRawAsync(string query, CancellationToken cancellationToken);
        Task<int> ExecuteSqlRawAsync(string query);
    }
}
