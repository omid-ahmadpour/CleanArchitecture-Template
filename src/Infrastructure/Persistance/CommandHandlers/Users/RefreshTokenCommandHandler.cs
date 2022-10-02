using CleanTemplate.Application.Users.Command.RefreshToken;
using CleanTemplate.Common;
using CleanTemplate.Domain.Entities.Users;
using CleanTemplate.Domain.IRepositories;
using CleanTemplate.Persistance.Jwt;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Persistance.CommandHandlers.Users
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(IUserRepository userRepository,
                                   IJwtService jwtService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }
        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var userId = _jwtService.ValidateJwtAccessTokenAsync(request.AccessToken);
            if (userId == null)
                throw new CleanArchAppException("AccessToken is not valid");

            var user = await _userRepository.GetByIdAsync(cancellationToken, userId);
            if (user.RefreshToken != request.RefreshToken)
                throw new CleanArchAppException("RefreshToken is not valid");

            var jwt = await _jwtService.GenerateAsync(user);
            user.RefreshToken = jwt.refresh_token;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(jwt.refreshToken_expiresIn);
            await _userRepository.UpdateAsync(user, cancellationToken);
            return new RefreshTokenResponse
            {
                AccessToken = jwt.access_token,
                RefreshToken = jwt.refresh_token
            };
        }
    }
}
