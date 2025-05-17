using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace CosmosOdyssey.REST.Data
{
    public class DataSeeder : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DataSeeder(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("DataSeeder started");

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            Console.WriteLine($"Using connection string: {config.GetConnectionString("DefaultConnection")}");

            try
            {
                if (await context.Database.CanConnectAsync(cancellationToken))
                {
                    Console.WriteLine("Database connection successful");

                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"RouteInfos\" DROP CONSTRAINT IF EXISTS \"FK_RouteInfos_Planets_FromId\";");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"RouteInfos\" DROP CONSTRAINT IF EXISTS \"FK_RouteInfos_Planets_ToId\";");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Providers\" DROP CONSTRAINT IF EXISTS \"FK_Providers_Legs_LegId\";");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Legs\" DROP CONSTRAINT IF EXISTS \"FK_Legs_RouteInfos_RouteInfoId\";");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Legs\" DROP CONSTRAINT IF EXISTS \"FK_Legs_PriceLists_PriceListId\";");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Providers\" DROP CONSTRAINT IF EXISTS \"FK_Providers_Companies_CompanyId\";");


                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"RouteInfos\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"RouteInfos\" ALTER COLUMN \"FromId\" TYPE uuid USING \"FromId\"::uuid;");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"RouteInfos\" ALTER COLUMN \"ToId\" TYPE uuid USING \"ToId\"::uuid;");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Providers\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Providers\" ALTER COLUMN \"CompanyId\" TYPE uuid USING \"CompanyId\"::uuid;");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Providers\" ALTER COLUMN \"LegId\" TYPE uuid USING \"LegId\"::uuid;");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"PriceLists\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Planets\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Legs\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Legs\" ALTER COLUMN \"RouteInfoId\" TYPE uuid USING \"RouteInfoId\"::uuid;");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Legs\" ALTER COLUMN \"PriceListId\" TYPE uuid USING \"PriceListId\"::uuid;");
                    await context.Database.ExecuteSqlRawAsync("ALTER TABLE \"Companies\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");

                    await dataService.SyncTravelPricesAsync();

                    Console.WriteLine("DataSeeder completed successfully");
                    return;
                }
                else
                {
                    Console.WriteLine("Database connection failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DataSeeder failed: {ex.Message}");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("DataSeeder stopped");
            return Task.CompletedTask;
        }
    }
}