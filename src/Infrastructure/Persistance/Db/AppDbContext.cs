namespace Persistance.Db
{
    using Domain.Entities;
    using Domain.Entities.dbo.Products;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
