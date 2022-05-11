using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Domain.Interfaces
{
    public interface IGenerationRepository
    {
        void AddGeneration(Generation generation);
        List<object> GetGenerationsByMonth(IEnumerable<string> months);
    }
}
