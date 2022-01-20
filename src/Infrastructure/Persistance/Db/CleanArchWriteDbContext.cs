using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Persistance.Db
{
    public class CleanArchWriteDbContext : AppDbContext
    {
        public CleanArchWriteDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public CleanArchWriteDbContext() { }
    }
}