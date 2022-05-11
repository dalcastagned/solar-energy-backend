using Microsoft.EntityFrameworkCore;
using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Data.Context
{
    public class SolarPlantContext : DbContext
    {
        public SolarPlantContext(DbContextOptions<SolarPlantContext> options) : base(options)
        {
        }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Generation> Generations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Plant>(e =>
            {
                e.HasKey(p => p.Id);
                e.HasMany(p => p.Generations)
                .WithOne()
                .HasForeignKey(g => g.IdPlant)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Generation>(e =>
            {
                e.HasKey(g => g.Id);
            });
        }


    }
}