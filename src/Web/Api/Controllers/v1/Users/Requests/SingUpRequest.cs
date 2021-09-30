using Domain.Entities.Users;

namespace Api.Controllers.v1.Users.Requests
{
    public class SingUpRequest
    {
        public string UserName { get; set; }

        public string Email { get; set; }
        
        public string Password { get; set; }

        public string FullName { get; set; }

        public int Age { get; set; }

        public GenderType Gender { get; set; }
    }
}
