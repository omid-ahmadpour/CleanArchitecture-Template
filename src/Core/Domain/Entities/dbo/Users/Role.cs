using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.dbo.Users
{
    public class Role : IdentityRole<int>, IEntity
    {
        public string Description { get; set; }
    }
}
