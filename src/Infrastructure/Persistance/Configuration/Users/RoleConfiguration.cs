using CleanTemplate.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTemplate.Persistence.Configuration.Users
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        }
    }
}
