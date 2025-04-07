using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using REST.Models;

namespace CosmosOdyssey.REST.Data
{

    public class DataContext : DbContext
    {
        protected DataContext() : base() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<RouteInfo> RouteInfos { get; set; }
        public DbSet<Leg> Legs { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Provider>().ToTable("Providers");
            modelBuilder.Entity<Leg>().ToTable("Legs");
            modelBuilder.Entity<RouteInfo>().ToTable("RouteInfos");
            modelBuilder.Entity<PriceList>().ToTable("PriceLists");
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<Planet>().ToTable("Planets");


            modelBuilder.Entity<PriceList>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasConversion<string>();

                entity.HasMany(p => p.Legs)
                      .WithOne()
                      .HasForeignKey(l => l.PriceListId);
            });

            modelBuilder.Entity<Leg>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Id).HasConversion<string>();

                entity.HasOne(l => l.RouteInfo)
                      .WithMany()
                      .HasForeignKey(l => l.RouteInfoId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(l => l.Providers)
                      .WithOne(p => p.Leg)
                      .HasForeignKey(p => p.LegId);

                entity.HasOne(l => l.PriceList)
                    .WithMany(p => p.Legs)
                    .HasForeignKey(l => l.PriceListId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RouteInfo>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).HasConversion<string>();

                entity.HasOne(r => r.From)
                      .WithMany()
                      .HasForeignKey(r => r.FromId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.To)
                      .WithMany()
                      .HasForeignKey(r => r.ToId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasConversion<string>();

                entity.HasOne(p => p.Company)
                      .WithMany(c => c.Providers)
                      .HasForeignKey(p => p.CompanyId);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).HasConversion<string>();
            });

            modelBuilder.Entity<Planet>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasConversion<string>();
            });
        }

        public class DesignTimeDataContextFactory : IDesignTimeDbContextFactory<DataContext>
        {
            public DataContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.Docker.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

                return new DataContext(optionsBuilder.Options);
            }
        }
    }

}