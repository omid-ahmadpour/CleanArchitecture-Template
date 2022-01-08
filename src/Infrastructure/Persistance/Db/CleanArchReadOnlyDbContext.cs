using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Persistance.Db
{
    public class CleanArchReadOnlyDbContext : AppDbContext
    {
        public CleanArchReadOnlyDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
