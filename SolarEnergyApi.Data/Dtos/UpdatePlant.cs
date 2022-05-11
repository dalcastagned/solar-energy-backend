namespace SolarEnergyApi.Data.Dtos
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
