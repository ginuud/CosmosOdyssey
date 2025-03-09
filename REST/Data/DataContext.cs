using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Models;
using Microsoft.EntityFrameworkCore;
using REST.Models;

namespace CosmosOdyssey.REST.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Pricelist> Pricelists { get; set; }
        public DbSet<RouteInfo> RouteInfos { get; set; }
        public DbSet<Leg> Legs { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=cosmos_db;Username=cosmos_user;Password=cosmos_password");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seo RouteInfo from ja to Planetitega
            modelBuilder.Entity<RouteInfo>()
                .HasOne(ri => ri.From)
                .WithMany()
                .HasForeignKey(ri => ri.FromId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RouteInfo>()
                .HasOne(ri => ri.To)
                .WithMany()
                .HasForeignKey(ri => ri.ToId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seo Provider Company-ga
            modelBuilder.Entity<Provider>()
                .HasOne(p => p.Company)
                .WithMany()
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seo Provider Leg-ga
            modelBuilder.Entity<Provider>()
                .HasOne(p => p.Leg)
                .WithMany(l => l.Providers)
                .HasForeignKey(p => p.LegId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seo Leg RouteInfo-ga
            modelBuilder.Entity<Leg>()
                .HasOne(l => l.RouteInfo)
                .WithMany()
                .HasForeignKey(l => l.RouteInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seo Leg Pricelist-ga
            modelBuilder.Entity<Leg>()
                .HasOne(l => l.Pricelist)
                .WithMany(p => p.Legs)
                .HasForeignKey(l => l.PricelistId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}