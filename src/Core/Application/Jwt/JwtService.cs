using Common;
using Common.General;
using Domain.Entities.dbo.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartG.Api.ApplicationServices.Jwt;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Jwt
{
    public class JwtService : IJwtService, IScopedDependency
    {
        private readonly SiteSettings _siteSetting;
        private readonly UserManager<User> _userManager;

        public JwtService(IOptionsSnapshot<SiteSettings> settings,
                          UserManager<User> userManager)
        {
            _siteSetting = settings.Value;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<AccessToken> GenerateAsync(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            //var currentUser = await userManager.FindByNameAsync(user.NormalizedUserName.ToUpper().Normalize());
            var currentUser = await _userManager.Users.FirstOrDefaultAsync(a => a.UserName == user.UserName);
            if (currentUser == null)
                throw new CleanArchAppException("نام کاربری یا رمز عبور اشتباه است");

            var claims = await GetClaimsAsync(currentUser);

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

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

            //string encryptedJwt = tokenHandler.WriteToken(securityToken);

            return new AccessToken(securityToken);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("UserName", user.UserName));
            claims.Add(new Claim("Email", user.Email));
            claims.Add(new Claim("UserId", user.Id.ToString()));

            var userRoles = await _userManager.GetRolesAsync(user);

            // Add roles as multiple claims
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //JwtRegisteredClaimNames.Sub
            //var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

            //var list = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    //new Claim(ClaimTypes.MobilePhone, "09123456987"),
            //    //new Claim(securityStampClaimType, user.SecurityStamp.ToString())
            //};

            return claims;
        }
    }
}
