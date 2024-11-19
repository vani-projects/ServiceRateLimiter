namespace Vani.Comminication.Service
{
    public class CleanupService : BackgroundService
    {
        private readonly IRateLimiterService _rateLimiter;
        private readonly int _cleanupInterval;

        public CleanupService(IRateLimiterService rateLimiter, IConfiguration configuration)
        {
            _rateLimiter = rateLimiter;
            _cleanupInterval = Convert.ToInt16(configuration["RateLimitsResourceExpiryInSeconds"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _rateLimiter.CleanupInactiveNumbers(TimeSpan.FromMinutes(_cleanupInterval));
                await Task.Delay(TimeSpan.FromMinutes(_cleanupInterval), stoppingToken);
            }
        }
    }
}
