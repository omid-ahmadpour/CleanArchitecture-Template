using Common;
using Domain.Entities.dbo.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistance.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Command.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(UserManager<User> userManager,
                                   IJwtService jwtService)
        {
            _userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
            _jwtService = jwtService ?? throw new System.ArgumentNullException(nameof(jwtService));
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                throw new CleanArchAppException("نام کاربری یا رمز عبور اشتباه است");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                throw new CleanArchAppException("نام کاربری یا رمز عبور اشتباه است");

            var jwt = await _jwtService.GenerateAsync(user);

            return new LoginResponse
            {
                accessToken = jwt.access_token,
                refreshToken = jwt.refresh_token
            };
        }
    }
}
