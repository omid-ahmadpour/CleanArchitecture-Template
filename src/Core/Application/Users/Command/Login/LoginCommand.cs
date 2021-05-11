using MediatR;

namespace Application.Users.Command.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Refresh_token { get; set; }
    }
}
