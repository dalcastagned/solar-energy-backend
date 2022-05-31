using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Domain.Dtos
{
    public class ReadUser {
        public ReadUser(User user)
        {
            Id = user.Id;
            Email = user.Email;
        }
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
