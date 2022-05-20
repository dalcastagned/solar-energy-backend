using Microsoft.EntityFrameworkCore;
using SolarEnergyApi.Data.Context;
using SolarEnergyApi.Domain.Dtos;
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

        public async Task Add(Generation generation)
        {
            _context.Generations.Add(generation);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReadGeneration>> GetAll(int plantId)
        {
            var generations = await _context.Generations
                .Where(g => g.IdPlant == plantId)
                .ToListAsync();
            return generations
                .Select(g => new ReadGeneration(g))
                .OrderByDescending(g => g.Id)
                .ToList();
        }

        public async Task<Generation> GetById(int plantId, int generationId)
        {
            return await _context.Generations
                .Where(g => g.IdPlant == plantId)
                .FirstOrDefaultAsync(g => g.Id == generationId);
        }
        public async Task Delete(Generation generation)
        {
            _context.Generations.Remove(generation);
            await _context.SaveChangesAsync();
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReadMonthGeneration>> GetByMonth(
            IEnumerable<string> months
        )
        {
            var generations = await _context.Generations
                .Where(g => g.Date >= DateTimeOffset.Now.AddMonths(-12))
                .ToListAsync();
            var result = new List<ReadMonthGeneration>();
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
                result.Add(new ReadMonthGeneration(month, sum));
            }
            return result.Reverse<ReadMonthGeneration>().ToList();
        }
    }
}
