using Canducci.Pagination;
using Microsoft.EntityFrameworkCore;
using SolarEnergyApi.Data.Context;
using SolarEnergyApi.Domain.Dtos;
using SolarEnergyApi.Domain.Entities;
using SolarEnergyApi.Domain.Interfaces;
using System.Linq;

namespace SolarEnergyApi.Domain.Services
{
    public class PlantRepository : IPlantRepository
    {
        private readonly SolarPlantContext _context;

        public PlantRepository(SolarPlantContext context)
        {
            _context = context;
        }

        public async Task Add(Plant plant)
        {
            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();
        }

        public async Task<ReadPlants> GetAll(int page, int limit, string? filter, bool? active)
        {
            if (page == 0)
                page = 1;
            if (limit == 0)
                limit = int.MaxValue;

            var plants = await _context.Plants
                .Include(x => x.Generations)
                .OrderByDescending(x => x.Id)
                .Where(x => active == null || x.Active == active)
                .Where(
                    x =>
                        string.IsNullOrEmpty(filter)
                        || x.Nickname.ToUpper().Contains(filter)
                        || x.Brand.ToUpper().Contains(filter)
                        || x.Model.ToUpper().Contains(filter)
                        || x.Place.ToUpper().Contains(filter)
                )
                .ToPaginatedRestAsync(page, limit);

            return new ReadPlants(plants);
        }

        public async Task<Plant?> GetById(int id)
        {
            return await _context.Plants
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task Delete(Plant plant)
        {
            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ReadPlantsCounts> GetPlantsCounts()
        {
            var activePlants = await _context.Plants.Where(x => x.Active).CountAsync();
            var inactivePlants = await _context.Plants.Where(x => x.Active == false).CountAsync();

            return new ReadPlantsCounts(activePlants, inactivePlants);
        }
    }
}
