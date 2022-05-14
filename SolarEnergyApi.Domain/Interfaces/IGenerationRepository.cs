using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Domain.Interfaces
{
    public interface IGenerationRepository
    {
        Task AddGeneration(Generation generation);
        Task<IEnumerable<object>> GetGenerationsByMonth(IEnumerable<string> months);
    }
}
