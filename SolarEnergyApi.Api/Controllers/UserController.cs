using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SolarEnergyApi.Data.Dtos;
using SolarEnergyApi.Domain.Entities;
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
        public async Task<IActionResult> GetUser(ReadUser user)
        {
            return Ok(user);
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> PostUser(AddUser model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
            };

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
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(
                    u => u.NormalizedEmail == login.Email.ToUpper()
                );
                var returnUser = new ReadUser(login.Email);

                return Ok(new { token = GenerateJwt(appUser).Result, user = returnUser });
            }
            return Unauthorized();
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
                new Claim(JwtRegisteredClaimNames.Sub, appUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, appUser.UserName)
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