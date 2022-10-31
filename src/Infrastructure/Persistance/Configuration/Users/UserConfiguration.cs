using CleanTemplate.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CleanTemplate.Persistence.Configuration.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.UserName).IsRequired().HasMaxLength(100);
        }
    }
}
