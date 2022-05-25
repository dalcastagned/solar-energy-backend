using Microsoft.AspNetCore.Identity;
using SolarEnergyApi.Domain.Dtos;
using SolarEnergyApi.Domain.Entities;
using SolarEnergyApi.Domain.Interfaces;

namespace SolarEnergyApi.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IdentityResult> SignUp(User user, string password)
        {
            return await _userRepository.SignUp(user, password);
        }

        public async Task<User> GetUser(string email) => await _userRepository.GetUser(email);

        public async Task<SignInResult> Login(User user, string password) => await _userRepository.Login(user, password);

        public async Task<IdentityResult> ChangePassword(User user, string oldPassword, string newPassword) => await _userRepository.ChangePassword(user, oldPassword, newPassword);

        public async Task<IdentityResult> AddToRole(User user, string role) => await _userRepository.AddToRole(user, role);

        public async Task<IEnumerable<ReadUser>> GetAllUsers() => await _userRepository.GetAllUsers();

        public async Task<IEnumerable<ReadRole>> GetAllRoles() => await _userRepository.GetAllRoles();
        public async Task<IdentityResult> AddToRoles(User user, IEnumerable<string> roles) => await _userRepository.AddToRoles(user, roles);
    }
}
