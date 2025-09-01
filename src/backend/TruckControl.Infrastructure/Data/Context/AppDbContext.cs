using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TruckControl.Domain.Entities;
using TruckControl.Domain.Enums;

namespace TruckBack.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Truck> Trucks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Truck>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Model)
                      .HasConversion(
                          new EnumToStringConverter<ModelEnum>()
                      )
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(t => t.ChassisNumber).HasMaxLength(100);
                entity.Property(t => t.Color).HasMaxLength(100);
                entity.Property(t => t.YearManufacture).IsRequired();
                entity.Property(t => t.Plant)
                      .HasConversion(
                          new EnumToStringConverter<PlantEnum>()
                      )
                      .IsRequired()
                      .HasMaxLength(20);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}