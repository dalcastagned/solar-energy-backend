using Microsoft.EntityFrameworkCore;
using SolarEnergyApi.Data.Context;
using SolarEnergyApi.Domain.Entities;
using SolarEnergyApi.Domain.Interfaces;

namespace SolarEnergyApi.Domain.Services
{
    public class GenerationRepository : IGenerationRepository
    {
        private readonly SolarPlantContext _context;

        public GenerationRepository(SolarPlantContext context)
        {
            _context = context;
        }

        public async Task AddGeneration(Generation generation)
        {
            _context.Generations.Add(generation);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<object>> GetGenerationsByMonth(IEnumerable<string> months)
        {
            var generations = await _context.Generations
                .Where(g => g.Date >= DateTimeOffset.Now.AddMonths(-12))
                .ToListAsync();
            var result = new List<object>();
            foreach (var month in months)
            {
                var monthGenerations = generations
                    .Where(g => g.Date.ToString("MM/yy") == month)
                    .ToList();
                double sum = 0.0;
                foreach (var generation in monthGenerations)
                {
                    sum += generation.GeneratePower;
                }
                result.Add(new { month, value = sum });
            }
            return result.Reverse<object>().ToList();
        }
    }
}
