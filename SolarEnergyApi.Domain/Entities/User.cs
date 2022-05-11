using Microsoft.AspNetCore.Identity;

namespace SolarEnergyApi.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}
