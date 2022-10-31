namespace CleanTemplate.Persistence.Db
{
    using CleanTemplate.Common.Utilities;
    using CleanTemplate.Domain.Entities;
    using Domain.Entities.Users;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public class AppDbContext : IdentityDbContext<User, Role, int>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public AppDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntity).Assembly;

            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntity).Assembly);
            modelBuilder.AddPluralizingTableNameConvention();
        }

        public async Task<int> ExecuteSqlRawAsync(string query, CancellationToken cancellationToken)
        {
            var result = await base.Database.ExecuteSqlRawAsync(query, cancellationToken);
            return result;
        }

        public async Task<int> ExecuteSqlRawAsync(string query) => await ExecuteSqlRawAsync(query, CancellationToken.None);
    }
}
