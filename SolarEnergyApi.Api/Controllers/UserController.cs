using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SolarEnergyApi.Domain.Dtos;
using SolarEnergyApi.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SolarPlants.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(
            IConfiguration configuration,
            UserManager<User> userManager,
            SignInManager<User> signInManager
        )
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetUser(ReadUser user)
        {
            return Ok(user);
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status201Created, description: "Created")]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, description: "Bad Request")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [SwaggerOperation(Summary = "Add new user")]
        public async Task<IActionResult> PostUser(AddUser model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
            };
            user.PasswordExpired = DateTime.Now.AddMonths(6).ToShortDateString();
            
            var returnUser = new ReadUser(model.Email);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Created(nameof(GetUser), returnUser);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Success")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [SwaggerOperation(Summary = "Login")]
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (result.Succeeded)
            {
                if (DateTime.Now > Convert.ToDateTime(user.PasswordExpired))
                {
                    return Unauthorized("Password expired");
                }
                
                var appUser = await _userManager.Users.FirstOrDefaultAsync(
                    u => u.NormalizedEmail == login.Email.ToUpper()
                );
                var returnUser = new ReadUser(login.Email);

                return Ok(new { token = GenerateJwt(appUser).Result, user = returnUser });
            }
            return Unauthorized("User or password incorrect");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Success")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status400BadRequest,
            description: "Bad Request"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [SwaggerOperation(Summary = "Reset Password")]
        public async Task<IActionResult> ResetPassword(string user, string oldPassword, string newPassword)
        {
            var userToReset = await _userManager.FindByEmailAsync(user);
            userToReset.PasswordExpired = DateTime.Now.AddMonths(6).ToShortDateString();
            var result = await _userManager.ChangePasswordAsync(userToReset, oldPassword, newPassword);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        private async Task<string> GenerateJwt(User appUser)
        {
            var key = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var roles = await _userManager.GetRolesAsync(appUser);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            return new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityTokenHandler().CreateToken(tokenDescriptor)
            );
        }
    }
}
