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
        private readonly ILogger<AuthController> _logger;

        public AuthController(
                              ITokenService tokenService,
                              UserManager<IdentityUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IConfiguration configuration,
                              ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = configuration;
            _logger = logger;
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
            _logger.LogInformation($"Failed login attempt for user: {loginDto.UserName}");
            return Unauthorized("Invalid username or password");

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
                    _logger.LogInformation($"Role {roleNome} created successfully");
                    return Ok("Role created successfully");
                }
                else
                {
                    _logger.LogError($"Error creating role {roleNome}: " +
                                     $"{string.Join(", ", roleResult.Errors.Select
                                     (e => e.Description))}");
                    return BadRequest("Error creating role");
                }
            }
            return BadRequest("Role already exists");
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
                    _logger.LogInformation($"User {email} added to role {roleName}");
                    return Ok();
                }
                else
                {
                    _logger.LogError($"Error adding user {email} to role {roleName}:" +
                                     $"{string.Join(", ", result.Errors.Select
                                     (e => e.Description))}");
                    return BadRequest("Error adding user to role");
                }
            }
            _logger.LogInformation($"User {email} not found");
            return BadRequest("User not found");
        }

        [HttpPost]
        [Route("registerDto")]

        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.UserName!);

            if (userExists != null)
            {
                return BadRequest("User already exists");
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
                _logger.LogError($"Error registering user {registerDto.UserName}:" +
                                                     $"{string.Join(", ", result.Errors.Select
                                                     (e => e.Description))}");
                return BadRequest("Error registering user");
            }
            _logger.LogInformation($"User {registerDto.UserName} regisered successfuly");
            return Ok("User registered successfully");
        }
    }
}
