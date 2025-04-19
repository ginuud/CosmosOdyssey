using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosOdyssey.REST.Data;

namespace CosmosOdyssey.REST.Services
{
    public class TravelPriceSyncService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TravelPriceSyncService> _logger;
        private readonly TimeSpan _syncInterval = TimeSpan.FromMinutes(2);

        public TravelPriceSyncService(IServiceScopeFactory scopeFactory, ILogger<TravelPriceSyncService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TravelPriceSyncService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();

                    //_logger.LogInformation("Syncing travel prices...");
                    await dataService.SyncTravelPricesAsync();
                    _logger.LogDebug("Travel prices synced successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while syncing travel prices.");
                }

                await Task.Delay(_syncInterval, stoppingToken);
            }

            _logger.LogInformation("TravelPriceSyncService is stopping.");
        }
    }
}