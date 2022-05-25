namespace SolarEnergyApi.Domain.Dtos
{
    public record AddUserRoles(string Email, IEnumerable<string> Roles)
    {
    }
}