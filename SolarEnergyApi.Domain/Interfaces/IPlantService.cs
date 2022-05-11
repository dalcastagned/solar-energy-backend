using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Domain.Interfaces
{
    public interface IPlantService
    {
        object GetAll(int page, int pageSize, string? filter, bool? active);
        Plant? GetById(int id);
        void Add(Plant plant);
        void Update();
        void Delete(Plant plant);
    }
}
