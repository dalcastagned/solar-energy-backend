using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarEnergyApi.Api.Utils;
using SolarEnergyApi.Domain.Dtos;
using SolarEnergyApi.Domain.Entities;
using SolarEnergyApi.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SolarPlants.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public UserController(
            IUserService userService,
            IConfiguration configuration,
            UserManager<User> userManager
        )
        {
            _userService = userService;
            _configuration = configuration;
            _userManager = userManager;
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
                PasswordExpired = DateTime.Now.AddMonths(6).ToShortDateString()
            };
            var result = await _userService.SignUp(user, model.Password);

            if (result.Succeeded)
            {
                try
                {
                    await _userService.AddToRole(user, "visitor");
                    return Ok(new { user = user.Email, role = "visitor" });
                }
                catch
                {
                    return Ok(new { user = user.Email });
                }
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
            var user = await _userService.GetUser(login.Email);
            var result = await _userService.Login(user, login.Password);

            if (result.Succeeded)
            {
                if (DateTime.Now > Convert.ToDateTime(user.PasswordExpired))
                {
                    return Unauthorized("Password expired");
                }
                return Ok(
                    new
                    {
                        token = new GenerateJWT()
                            .Generate(user, _configuration, _userManager)
                            .Result,
                        user = login.Email
                    }
                );
            }
            return Unauthorized("User or password incorrect");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Success")]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, description: "Bad Request")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [SwaggerOperation(Summary = "Reset Password")]
        public async Task<IActionResult> ResetPassword(
            string user,
            string oldPassword,
            string newPassword
        )
        {
            var userToReset = await _userService.GetUser(user);
            userToReset.PasswordExpired = DateTime.Now.AddMonths(6).ToShortDateString();
            var result = await _userService.ChangePassword(userToReset, oldPassword, newPassword);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("get-users")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Success",
            type: typeof(ReadUser)
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [SwaggerOperation(Summary = "Return list of users with roles")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
    }
}
