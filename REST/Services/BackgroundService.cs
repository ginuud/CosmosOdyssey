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
        public TravelPriceSyncService(IServiceScopeFactory scopeFactory, ILogger<TravelPriceSyncService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TravelPriceSyncService is starting.");

            DateTime? nextSyncTime = null;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
                    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                    await dataService.SyncTravelPricesAsync();
                    _logger.LogDebug("Travel prices synced successfully.");

                    var latestPriceListValidUntil = context.PriceLists
                        .OrderByDescending(pl => pl.ValidUntil)
                        .Select(pl => pl.ValidUntil)
                        .FirstOrDefault();

                    if (latestPriceListValidUntil != default)
                    {
                        nextSyncTime = latestPriceListValidUntil.AddSeconds(3);
                    }
                    else
                    {
                        _logger.LogWarning("No valid price list found. Will retry in 1 minute.");
                        nextSyncTime = DateTime.UtcNow.AddMinutes(1);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while syncing travel prices.");
                    nextSyncTime = DateTime.UtcNow.AddMinutes(1);
                }

                var delay = nextSyncTime.HasValue
                ? nextSyncTime.Value - DateTime.UtcNow
                : TimeSpan.FromMinutes(1);

                if (delay < TimeSpan.Zero)
                    delay = TimeSpan.Zero;

                _logger.LogInformation($"Next sync scheduled in {delay.TotalSeconds:F0} seconds");

                try
                {
                    await Task.Delay(delay, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }

            _logger.LogInformation("TravelPriceSyncService is stopping.");
        }
    }
}