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

        public void AddGeneration(Generation generation) => _generationRepository.AddGeneration(generation);
        
        public List<object> GetGenerationsByMonth(IEnumerable<string> months) => _generationRepository.GetGenerationsByMonth(months);
    }
}
