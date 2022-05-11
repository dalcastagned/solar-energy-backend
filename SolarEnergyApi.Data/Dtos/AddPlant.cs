namespace SolarEnergyApi.Data.Dtos
{
    public record AddPlant(
        string Nickname,
        string Place,
        string Brand,
        string Model,
        bool Active
    )
    { }
}
