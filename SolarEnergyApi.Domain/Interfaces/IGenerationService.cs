using SolarEnergyApi.Domain.Dtos;
using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Domain.Interfaces
{
    public interface IGenerationService
    {
        Task Add(Generation generation);
        Task<ReadGenerations> GetAll(int page, int limit, int plantId, DateTime? startDate, DateTime? endDate);
        Task<Generation> GetById(int plantId, int generationId);
        Task Update();
        Task Delete(Generation generation);
        Task<IEnumerable<ReadMonthGeneration>> GetByMonth(IEnumerable<string> months);
    }
}
