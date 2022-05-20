using SolarEnergyApi.Domain.Dtos;
using SolarEnergyApi.Domain.Entities;
using SolarEnergyApi.Domain.Interfaces;

namespace SolarEnergyApi.Domain.Services
{
    public class GenerationService : IGenerationService
    {
        private readonly IGenerationRepository _generationRepository;

        public GenerationService(IGenerationRepository generationRepository)
        {
            _generationRepository = generationRepository;
        }

        public async Task Add(Generation generation) =>
            await _generationRepository.Add(generation);

        public async Task<IEnumerable<ReadGeneration>> GetAll(int plantId) =>
            await _generationRepository.GetAll(plantId);

        public async Task<Generation> GetById(int plantId, int generationId)
        {
            var generation = await _generationRepository.GetById(plantId, generationId);
            if (generation is null)
                throw new KeyNotFoundException("Generation not found");
            return generation;
        }

        public async Task Delete(Generation generation) => await _generationRepository.Delete(generation);

        public async Task Update() => await _generationRepository.Update();

        public async Task<IEnumerable<ReadMonthGeneration>> GetByMonth(
            IEnumerable<string> months
        ) => await _generationRepository.GetByMonth(months);
    }
}
