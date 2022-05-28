using Canducci.Pagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SolarEnergyApi.Data.Context;
using SolarEnergyApi.Domain.Dtos;
using SolarEnergyApi.Domain.Entities;
using SolarEnergyApi.Domain.Interfaces;

namespace SolarEnergyApi.Domain.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(
            IConfiguration configuration,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
        SignInManager<User> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> SignUp(User user, string password) => await _userManager.CreateAsync(user, password);

        public async Task<User> GetUser(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<SignInResult> SignIn(User user, string password) => await _signInManager.CheckPasswordSignInAsync(user, password, false);

        public async Task<SignInResult> Login(User user, string password) => await _signInManager.PasswordSignInAsync(user.Email, password, false, false);

        public async Task<IdentityResult> ChangePassword(User user, string currentPassword, string newPassword) => await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        public async Task<IdentityResult> AddToRole(User user, string role) => await _userManager.AddToRoleAsync(user, role);

        public async Task<IEnumerable<ReadUser>> GetAllUsers() => await _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Select(u => new ReadUser(u)).ToListAsync();

        public async Task<IEnumerable<ReadRole>> GetAllRoles() => await _roleManager.Roles.Select(r => new ReadRole(r)).ToListAsync();
        public async Task<IdentityResult> AddToRoles(User user, IEnumerable<string> roles) => await _userManager.AddToRolesAsync(user, roles);
        public async Task<IEnumerable<String>> GetUserRoles(User user) => await _userManager.GetRolesAsync(user);
    }
}
