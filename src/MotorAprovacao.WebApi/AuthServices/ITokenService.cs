using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MotorAprovacao.WebApi.AuthServices
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims);


        ClaimsPrincipal GetPricipalFromExpiredToken(string token);
    }
}
