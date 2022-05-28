namespace SolarEnergyApi.Domain.Dtos
{
    public record ResetPassword(string CurrentPassword, string NewPassword)
    {
    }
}