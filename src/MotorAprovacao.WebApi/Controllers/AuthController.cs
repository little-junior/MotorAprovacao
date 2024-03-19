using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotorAprovacao.WebApi.AuthDTOs;
using MotorAprovacao.WebApi.AuthServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MotorAprovacao.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthController(
                              ITokenService tokenService,
                              UserManager<IdentityUser> userManager,
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
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }

            return Unauthorized();

        }


        [HttpPost]
        [Route("createRole")]
        //[Authorize(Policy = "ManagerOnly")]


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
                    return Ok("Função criada com sucesso");
                }
                else
                {
                    //to do é possivel registrar logger
                    //to do ajustar retorno de status
                    return BadRequest("Erro na criação");
                }
            }
            return BadRequest("Função inexistente");
        }

        [HttpPost]
        [Route("userRole")]
        //[Authorize(Policy = "ManagerOnly")]
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
        //[Authorize(Policy = "AllRoles")]

        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.UserName!);

            if (userExists != null)
            {
                return BadRequest("Usuario já existente");
            }

            IdentityUser user = new()
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.UserName
            };


            var result = await _userManager.CreateAsync(user, registerDto.Password!);
            if (!result.Succeeded)
            {
                return BadRequest("Erro na criação");
            }

            return Ok("Usuário criado com sucesso");
        }
    }
}
