using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotorAprovacao.Data.EF;
using MotorAprovacao.Models.Entities;


//using MotorAprovacao.Models.Entities;
using MotorAprovacao.WebApi.AuthDTOs;
using MotorAprovacao.WebApi.AuthServices;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
//using MotorAprovacao.Models

namespace MotorAprovacao.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthController(ITokenService tokenService,
                              UserManager<ApplicationUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName!);

            if (user is not null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims, _config);
                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_config["JWT:RefreshTokenValidityInMinutes"],
                                    out int refreshTokenValidityInMinutes);

                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
                user.RefreshToken = refreshToken;
                

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
            else
            {
                return Unauthorized();
            }
        }


        [HttpPost]
        [Route("createRole")]
        public async Task<IActionResult> CreateRole(string roleNome)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleNome);
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleNome));

                if (roleResult.Succeeded)
                {
                    //to do é possivel registrar logger
                    //to do ajustar retorno de status
                    return Ok();
                }
                else
                {
                    //to do é possivel registrar logger
                    //to do ajustar retorno de status
                    return BadRequest();
                }
            }
            return BadRequest("Função inexistente");
        }

        [HttpPost]
        [Route("userRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    //to do é possivel registrar logger
                    //to do ajustar retorno de status
                    return Ok();
                }
                else
                {
                    //to do é possivel registrar logger
                    //to do ajustar retorno de status
                    return BadRequest();
                }
            }
            //to do ajustar retorno do erro
            return BadRequest();
        }

        [HttpPost]
        [Route("registerDto")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.UserName);

            if (userExists != null)
            {
                return BadRequest();
            }

             ApplicationUser user = new()
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.UserName
            };


            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken(TokenDTO tokenDto)
        {
            if (tokenDto is null)
            {
                return BadRequest("Acesso inválido!");
            }

            string? accessToken = tokenDto.AcessToken ??
               throw new ArgumentException(nameof(tokenDto));

            string? refreshToken = tokenDto.RefreshToken ??
                throw new ArgumentException(nameof(tokenDto));

            var principal = _tokenService.GetPricipalFromExpiredToken(accessToken!, _config);

            if (principal == null)
            {
                return BadRequest("Acesso inválido!");
            }

            string userName = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Acesso inválido!");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _config);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
            });
        }

    }
}
