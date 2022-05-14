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

        public async Task AddGeneration(Generation generation) => await _generationRepository.AddGeneration(generation);
        
        public async Task<IEnumerable<object>> GetGenerationsByMonth(IEnumerable<string> months) => await _generationRepository.GetGenerationsByMonth(months);
    }
}
