using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MotorAprovacao.WebApi.AuthServices
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims,
                                             IConfiguration _config);


        ClaimsPrincipal GetPricipalFromExpiredToken(string token,
                                                    IConfiguration _config);
    }
}
