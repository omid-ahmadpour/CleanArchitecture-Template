using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Persistence.Db
{
    public class CleanArchWriteDbContext : AppDbContext
    {
        public CleanArchWriteDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public CleanArchWriteDbContext() { }
    }
}