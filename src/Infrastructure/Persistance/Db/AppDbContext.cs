namespace Persistance.Db
{
    using Common.Utilities;
    using Domain.Entities;
    using Domain.Entities.dbo.Products;
    using Domain.Entities.dbo.Users;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class AppDbContext : IdentityDbContext<User, Role, int>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntity).Assembly;

            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntity).Assembly);
        }

        public async Task<int> ExecuteSqlRawAsync(string query, CancellationToken cancellationToken)
        {
            var result = await base.Database.ExecuteSqlRawAsync(query, cancellationToken);
            return result;
        }

        public async Task<int> ExecuteSqlRawAsync(string query) => await ExecuteSqlRawAsync(query, CancellationToken.None);
    }
}
