using CleanTemplate.Application.Users.Command.Login;
using CleanTemplate.Common;
using CleanTemplate.Common.Exceptions;
using CleanTemplate.Domain.Entities.Users;
using CleanTemplate.Persistance.Jwt;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Persistance.CommandHandlers.Users
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(UserManager<User> userManager,
                                   IJwtService jwtService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                throw new CleanArchAppException("username or password is incorrect");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                throw new CleanArchAppException("username or password is incorrect");

            var jwt = await _jwtService.GenerateAsync(user);

            return new LoginResponse
            {
                accessToken = jwt.access_token,
                refreshToken = jwt.refresh_token
            };
        }
    }
}
