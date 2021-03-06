// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolarEnergyApi.Data.Context;

#nullable disable

namespace SolarEnergyApi.Data.Migrations
{
    [DbContext(typeof(SolarPlantContext))]
    [Migration("20220511221257_initialMigrations")]
    partial class initialMigrations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SolarEnergyApi.Domain.Entities.Generation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("GeneratePower")
                        .HasColumnType("float");

                    b.Property<int>("IdPlant")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPlant");

                    b.ToTable("Generations");
                });

            modelBuilder.Entity("SolarEnergyApi.Domain.Entities.Plant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Place")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("SolarEnergyApi.Domain.Entities.Generation", b =>
                {
                    b.HasOne("SolarEnergyApi.Domain.Entities.Plant", null)
                        .WithMany("Generations")
                        .HasForeignKey("IdPlant")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SolarEnergyApi.Domain.Entities.Plant", b =>
                {
                    b.Navigation("Generations");
                });
#pragma warning restore 612, 618
        }
    }
}
