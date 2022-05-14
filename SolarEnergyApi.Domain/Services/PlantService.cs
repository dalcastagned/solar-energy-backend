using Canducci.Pagination;
using SolarEnergyApi.Domain.Dtos;
using SolarEnergyApi.Domain.Entities;
using SolarEnergyApi.Domain.Interfaces;

namespace SolarEnergyApi.Domain.Services
{
    public class PlantService : IPlantService
    {
        private readonly IPlantRepository _plantRepository;

        public PlantService(IPlantRepository plantRepository) => _plantRepository = plantRepository;

        public async Task Add(Plant plant) => await _plantRepository.Add(plant);

        public async Task<ReadPlants> GetAll(int page, int limit, string? filter, bool? active) => await _plantRepository.GetAll(page, limit, filter, active);

        public async Task<Plant?> GetById(int id)
        {
            var plant = await _plantRepository.GetById(id);
            if (plant is null) throw new KeyNotFoundException("Planta não encontrada");
            return plant;
        }
        
        public async Task Delete(Plant plant) => await _plantRepository.Delete(plant);

        public async Task Update() => await _plantRepository.Update();
    }
}
