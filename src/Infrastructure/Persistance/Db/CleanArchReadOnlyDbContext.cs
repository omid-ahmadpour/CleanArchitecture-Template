using Microsoft.EntityFrameworkCore;

namespace Persistance.Db
{
    public class CleanArchReadOnlyDbContext : AppDbContext
    {
        public CleanArchReadOnlyDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
