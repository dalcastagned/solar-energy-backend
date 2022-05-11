using Microsoft.AspNetCore.Identity;

namespace SolarEnergyApi.Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}
