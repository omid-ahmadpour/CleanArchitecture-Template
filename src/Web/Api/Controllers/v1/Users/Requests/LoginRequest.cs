namespace Api.Controllers.v1.Users.Requests
{
    public class LoginRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Refresh_token { get; set; }
    }
}
