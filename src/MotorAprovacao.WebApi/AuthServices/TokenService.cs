using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MotorAprovacao.WebApi.AuthServices
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                        throw new InvalidOperationException("Invalid secret key");
            
            var privateKey = Encoding.UTF32.GetBytes(key);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
                                      SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(_config.GetSection("JWT")
                                                     .GetValue<double>("TokenValisityInDays")),
                
                Audience = _config.GetSection("JWT").GetValue<string>("ValidAudience"),

                Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"), 
                
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return token;
        }
 

        public ClaimsPrincipal GetPricipalFromExpiredToken(string token, IConfiguration _config)
        {
            var secretKey = _config["JWT:SecretKey"] ??
                             throw new InvalidOperationException("Chave Inválida");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                                       Encoding.UTF32.GetBytes(secretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var primary = tokenHandler.ValidateToken(token, tokenValidationParameters,
                                                     out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                                !jwtSecurityToken.Header.Alg.Equals(
                                    SecurityAlgorithms.HmacSha256,
                                    StringComparison.InvariantCultureIgnoreCase)) 
            {
                throw new SecurityTokenException("Token Inválido");
            }

            return primary;
        }
    }
}
