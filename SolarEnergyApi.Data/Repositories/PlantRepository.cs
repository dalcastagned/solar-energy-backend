using Canducci.Pagination;
using Microsoft.EntityFrameworkCore;
using SolarEnergyApi.Data.Context;
using SolarEnergyApi.Data.Dtos;
using SolarEnergyApi.Domain.Entities;
using SolarEnergyApi.Domain.Interfaces;

namespace SolarEnergyApi.Domain.Services
{
    public class PlantRepository : IPlantRepository
    {
        private readonly SolarPlantContext _context;

        public PlantRepository(SolarPlantContext context)
        {
            _context = context;
        }

        public void Add(Plant plant)
        {
            _context.Plants.Add(plant);
            _context.SaveChanges();
        }

        public object GetAll(int page, int pageSize, string? filter, bool? active)
        {
            var plants = _context.Plants.Include(plant => plant.Generations).AsQueryable();

            if (active != null)
            {
                plants = plants.Where(plant => plant.Active == active);
            }
            if (!string.IsNullOrEmpty(filter))
                plants = plants.Where(
                    x =>
                        x.Nickname.ToUpper().Contains(filter.ToUpper())
                        || x.Brand.ToUpper().Contains(filter.ToUpper())
                        || x.Model.ToUpper().Contains(filter.ToUpper())
                        || x.Place.ToUpper().Contains(filter.ToUpper())
                );
            if (page != 0)
            {
                return new ReadPlants(plants.ToPaginatedRestAsync(page, pageSize));
            }
            else
            {
                return new { plants = plants.ToList() };
            }
        }

        public Plant? GetById(int id)
        {
            return _context.Plants
                .Include(plant => plant.Generations)
                .SingleOrDefault(p => p.Id == id);
        }

        public void Delete(Plant plant)
        {
            _context.Plants.Remove(plant);
            _context.SaveChanges();
        }

        public void Update()
        {
            _context.SaveChanges();
        }
    }
}
