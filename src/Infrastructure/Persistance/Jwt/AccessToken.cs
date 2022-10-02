using System;
using System.IdentityModel.Tokens.Jwt;

namespace CleanTemplate.Persistance.Jwt
{
    public class AccessToken
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int refreshToken_expiresIn { get; set; }

        public AccessToken(JwtSecurityToken securityToken)
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            token_type = "Bearer";
            expires_in = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
        }
        public AccessToken(JwtSecurityToken securityToken,string refreshToken, int refreshTokenExpiresIn)
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            token_type = "Bearer";
            expires_in = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
            refresh_token = refreshToken;
            refreshToken_expiresIn = refreshTokenExpiresIn;
        }
    }
}
