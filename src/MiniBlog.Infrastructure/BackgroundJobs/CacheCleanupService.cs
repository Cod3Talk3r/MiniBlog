using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace MiniBlog.Infrastructure.BackgroundJobs
{
    public class StatsLoggerService : BackgroundService
    {
        private readonly ILogger<StatsLoggerService> _logger;

        public StatsLoggerService(ILogger<StatsLoggerService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("MiniBlog is alive at {Time}", DateTimeOffset.UtcNow);
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}