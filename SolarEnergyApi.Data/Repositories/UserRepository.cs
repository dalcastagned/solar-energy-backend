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
        private readonly SignInManager<User> _signInManager;

        public UserRepository(
            IConfiguration configuration,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> SignUp(User user, string password) => await _userManager.CreateAsync(user, password);

        public async Task<User> GetUser(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<SignInResult> SignIn(User user, string password) => await _signInManager.CheckPasswordSignInAsync(user, password, false);

        public async Task<SignInResult> Login(User user, string password) => await _signInManager.PasswordSignInAsync(user.Email, password, false, false);

        public async Task<IdentityResult> ChangePassword(User user, string currentPassword, string newPassword) => await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        public async Task AddToRole(User user, string role) => await _userManager.AddToRoleAsync(user, role);
    }
}
