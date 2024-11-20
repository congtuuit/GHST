using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Services
{
    public abstract class CronJobService : BackgroundService
    {
        private readonly ILogger<CronJobService> _logger;
        private readonly TimeSpan _interval;

        protected CronJobService(ILogger<CronJobService> logger, TimeSpan interval)
        {
            _logger = logger;
            _interval = interval;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CronJobService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWorkAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing cron job.");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("CronJobService is stopping.");
        }

        public abstract Task DoWorkAsync(CancellationToken cancellationToken);
    }
}
