namespace CleanTemplate.Application.Users.Command.Login
{
    public class LoginResponse
    {
        public readonly static LoginResponse Empty = new LoginResponse();

        public string accessToken { get; set; }

        public string refreshToken { get; set; }
    }
}
