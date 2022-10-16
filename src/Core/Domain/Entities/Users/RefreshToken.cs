using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTemplate.Domain.Entities.Users
{
    public class RefreshToken : IEntity<int>
    {
        public int Id { get; set; }

        [ForeignKey(name: nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
        public DateTime ExpiryTime { get; set; }
    }
}
