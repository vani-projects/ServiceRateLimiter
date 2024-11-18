using StackExchange.Redis;
using Vani.Comminication.Contracts;

namespace Vani.Comminication.Helper
{
    public class RedisRateLimiter : IRateLimiter
    {
        private readonly IDatabase _redis;
        private readonly int _maxPerNumberPerSecond;
        private readonly int _maxPerAccountPerSecond;

        public RedisRateLimiter(IDatabase redis, int maxPerNumberPerSecond, int maxPerAccountPerSecond)
        {
            _redis = redis;
            _maxPerNumberPerSecond = maxPerNumberPerSecond;
            _maxPerAccountPerSecond = maxPerAccountPerSecond;
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
            throw new NotImplementedException();
        }
    }
}
