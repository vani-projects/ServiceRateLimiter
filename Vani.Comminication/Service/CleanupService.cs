
using Vani.Comminication.Helper;

namespace Vani.Comminication.Service
{
    public class CleanupService : BackgroundService
    {
        private readonly InMemoryRateLimiter _rateLimiter;

        public CleanupService(InMemoryRateLimiter rateLimiter)
        {
            _rateLimiter = rateLimiter;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _rateLimiter.CleanupInactiveNumbers(TimeSpan.FromMinutes(10));
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}
