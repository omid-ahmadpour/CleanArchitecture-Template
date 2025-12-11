using CleanTemplate.Common;
using CleanTemplate.Common.General;
using CleanTemplate.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CleanTemplate.Persistence.Jwt
{
    public class JwtService : IJwtService, IScopedDependency
    {
        private readonly SiteSettings _siteSetting;
        private readonly UserManager<User> _userManager;
        // Lazy initialization of byte arrays from configuration keys (scoped lifetime, thread-safe by default)
        // JWT keys are intentionally cached as they should not change at runtime (would invalidate all tokens)
        private readonly Lazy<byte[]> _secretKey;
        private readonly Lazy<byte[]> _encryptionKey;

        public JwtService(IOptionsSnapshot<SiteSettings> settings,
                          UserManager<User> userManager)
        {
            _siteSetting = settings.Value;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _secretKey = new Lazy<byte[]>(() => Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey));
            _encryptionKey = new Lazy<byte[]>(() => Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey));
        }

        public async Task<AccessToken> GenerateAsync(User user)
        {
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey.Value), SecurityAlgorithms.HmacSha256Signature);
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(_encryptionKey.Value), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await GetClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSetting.JwtSettings.Issuer,
                Audience = _siteSetting.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

            return new AccessToken(securityToken: securityToken,
                refreshToken: GenerateRefreshToken(),
                refreshTokenExpiresIn: _siteSetting.JwtSettings.RefreshTokenValidityInDays);
        }

        public int? ValidateJwtAccessTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secretKey.Value),
                    TokenDecryptionKey = new SymmetricSecurityKey(_encryptionKey.Value),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtSecurityToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value);
                return userId;
            }
            catch
            {
                return null;
            }
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>(capacity: 2 + userRoles.Count)
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
