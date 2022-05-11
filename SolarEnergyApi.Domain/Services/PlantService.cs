using SolarEnergyApi.Domain.Entities;
using SolarEnergyApi.Domain.Interfaces;

namespace SolarEnergyApi.Domain.Services
{
    public class PlantService : IPlantService
    {
        private readonly IPlantRepository _plantRepository;

        public PlantService(IPlantRepository plantRepository) => _plantRepository = plantRepository;

        public void Add(Plant plant) => _plantRepository.Add(plant);

        public object GetAll(int page, int pageSize, string? filter, bool? active) => _plantRepository.GetAll(page, pageSize, filter, active);

        public Plant? GetById(int id)
        {
            var plant = _plantRepository.GetById(id);
            if (plant is null) throw new KeyNotFoundException("Planta não encontrada");
            return plant;
        }
        
        public void Delete(Plant plant) => _plantRepository.Delete(plant);

        public void Update() => _plantRepository.Update();
    }
}
