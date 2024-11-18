
using Vani.Comminication.Contracts;
using Vani.Comminication.Helper;

namespace Vani.Comminication.Service
{
    public class CleanupService : BackgroundService
    {
        private readonly IRateLimiter _rateLimiter;

        public CleanupService(IRateLimiter rateLimiter)
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
