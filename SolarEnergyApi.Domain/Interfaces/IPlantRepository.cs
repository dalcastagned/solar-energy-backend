using Canducci.Pagination;
using SolarEnergyApi.Domain.Dtos;
using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Domain.Interfaces
{
    public interface IPlantRepository
    {
        Task<ReadPlants> GetAll(int page, int limit, string? filter, bool? active);
        Task<Plant?> GetById(int id);
        Task Add(Plant plant);
        Task Update();
        Task Delete(Plant plant);
        Task<ReadPlantsCounts> GetPlantsCounts();
    }
}
