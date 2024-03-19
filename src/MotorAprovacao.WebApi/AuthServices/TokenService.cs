using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MotorAprovacao.WebApi.AuthServices
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]);
            

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                      SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(_config.GetValue<double>("JWT:TokenValisityInDays")),
                Audience = _config["JWT:ValidAudience"],
                Issuer = _config["JWT:ValidIssuer"],               
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            
        }
 

        public ClaimsPrincipal GetPricipalFromExpiredToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]);


            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, tokenValidationParameters,out _);


        }
    }
}
