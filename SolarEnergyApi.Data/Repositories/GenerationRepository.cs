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

        public void AddGeneration(Generation generation)
        {
            _context.Generations.Add(generation);
            _context.SaveChanges();
        }

        public List<object> GetGenerationsByMonth(IEnumerable<string> months)
        {
            List<Generation> generations = _context.Generations
                .Where(g => g.Date >= DateTimeOffset.Now.AddMonths(-12))
                .ToList();
            List<object> result = new List<object>();
            foreach (var month in months)
            {
                List<Generation> monthGenerations = generations
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
