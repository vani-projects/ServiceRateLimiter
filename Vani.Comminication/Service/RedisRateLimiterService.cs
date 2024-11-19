using StackExchange.Redis;
using Vani.Comminication.Config;

namespace Vani.Comminication.Service
{
    public class RedisRateLimiterService : IRateLimiterService
    {
        private readonly IDatabase _redis;
        private readonly int _maxPerNumberPerSecond;
        private readonly int _maxPerAccountPerSecond;
        private const string REDIS_CONNECTION_STRING_KEY = "Redis";

        public RedisRateLimiterService(IConfiguration configuration)
        {
            ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(configuration.GetConnectionString(REDIS_CONNECTION_STRING_KEY));
            _redis = redisConnection.GetDatabase();

            var messageRateLimits = new MessageRateLimitsConfig();
            configuration.GetSection(MessageRateLimitsConfig.CONFIG_SECTION_TITLE).Bind(messageRateLimits);

            _maxPerNumberPerSecond = messageRateLimits.PhoneNumberRateLimitPerSecond;
            _maxPerAccountPerSecond = messageRateLimits.AccountRateLimitIPerSecond;
        }

        public async Task<bool> CanSendFromNumber(string phoneNumber)
        {
            var phoneKey = $"sms_limit:phone:{phoneNumber}";
            var accountKey = "sms_limit:account";

            // Increment per-phone counter with expiry of 1 second
            var phoneCount = await _redis.StringIncrementAsync(phoneKey);
            if (phoneCount == 1) await _redis.KeyExpireAsync(phoneKey, TimeSpan.FromSeconds(1));

            // Increment account-wide counter with expiry of 1 second
            var accountCount = await _redis.StringIncrementAsync(accountKey);
            if (accountCount == 1) await _redis.KeyExpireAsync(accountKey, TimeSpan.FromSeconds(1));

            if (phoneCount > _maxPerNumberPerSecond || accountCount > _maxPerAccountPerSecond)
            {
                // If either limit is exceeded, decrement counters to maintain accurate counts
                await _redis.StringDecrementAsync(phoneKey);
                await _redis.StringDecrementAsync(accountKey);
                return false;
            }
            return true;
        }

        public void CleanupInactiveNumbers(TimeSpan inactiveThreshold)
        {
            // Redis Cache takes care of the expiry by default (it expires the cache every second), so not explicit cleanup is required
        }
    }
}
