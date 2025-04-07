using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    if (await context.Database.CanConnectAsync(cancellationToken))
                    {
                        Console.WriteLine("Database connection successful");
                        await context.Database.MigrateAsync(cancellationToken);

                        await dataService.SyncTravelPricesAsync();

                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {i + 1}/5 failed: {ex.Message}");
                    await Task.Delay(5000, cancellationToken);
                }
            }

            throw new Exception("Failed to connect to database after 5 attempts");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}