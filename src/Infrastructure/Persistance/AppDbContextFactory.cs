namespace Persistance
{
    using Microsoft.EntityFrameworkCore;
    using Persistance.Db;

    public class AppDbContextFactory : DesignTimeDbContextFactoryBase<AppDbContext>
    {
        public AppDbContextFactory()
        { }

        protected override AppDbContext CreateNewInstance(DbContextOptions<AppDbContext> options)
        {
            return new AppDbContext(options);
        }
    }
}
