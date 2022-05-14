namespace SolarEnergyApi.Domain.Dtos
{
    public record UpdatePlant(
        string Nickname,
        string Place,
        string Brand,
        string Model,
        bool Active
    )
    { }
}
