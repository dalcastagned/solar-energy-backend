using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarEnergyApi.Domain.Entities;

namespace SolarEnergyApi.Data.Context
{
    public class SolarPlantContext
        : IdentityDbContext<
              User,
              Role,
              int,
              IdentityUserClaim<int>,
              UserRole,
              IdentityUserLogin<int>,
              IdentityRoleClaim<int>,
              IdentityUserToken<int>
          >
    {
        public SolarPlantContext(DbContextOptions<SolarPlantContext> options) : base(options) {
            this.Database.EnsureCreated();
        }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<Generation> Generations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedUsers(builder);
            
            builder.Entity<UserRole>(
                userRole =>
                {
                    userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                    userRole
                        .HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                    userRole
                        .HasOne(ur => ur.User)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
                }
            );

            builder.Entity<Plant>(
                e =>
                {
                    e.HasKey(p => p.Id);
                    e.ToTable("Plants");
                    e.HasMany(p => p.Generations)
                        .WithOne()
                        .HasForeignKey(g => g.IdPlant)
                        .OnDelete(DeleteBehavior.Cascade);
                }
            );

            builder.Entity<Generation>(
                e =>
                {
                    e.HasKey(g => g.Id);
                    e.ToTable("Generations");
                }
            );
        }

        private void SeedUsers(ModelBuilder builder)
        {
            User user = new User()
            {
                Id = 1,
                UserName = "suporte@suporte.com",
                NormalizedUserName = "SUPORTE@SUPORTE.COM",
                Email = "suporte@suporte.com",
                NormalizedEmail = "SUPORTE@SUPORTE.COM",
                EmailConfirmed = true,
            };
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, "suporte");
            user.PasswordExpired = DateTime.Now.AddDays(-1).ToShortDateString();

            builder.Entity<User>().HasData(user);
        }
    }
}
