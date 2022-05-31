using Microsoft.AspNetCore.Identity;
using SolarEnergyApi.Domain.Dtos;
using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> SignUp(User user, string password);
        Task<User> GetUser(string email);
        Task<SignInResult> Login(User user, string password);
        Task<IdentityResult> ChangePassword(User user, string oldPassword, string newPassword);
    }
}
