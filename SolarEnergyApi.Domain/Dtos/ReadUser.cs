using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Domain.Dtos
{
    public class ReadUser {
        public ReadUser(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Roles = user.UserRoles.Select(ur => new ReadRole(ur.Role)).ToList();
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<ReadRole> Roles { get; set; }
    }
}
