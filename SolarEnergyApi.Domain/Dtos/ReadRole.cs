using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Domain.Dtos
{
    public class ReadRole {
        public ReadRole(Role role)
        {
            Id = role.Id;
            Name = role.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
