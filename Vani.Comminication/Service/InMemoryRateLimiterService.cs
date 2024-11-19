using System.Collections.Concurrent;
using Vani.Comminication.Config;
using Vani.Comminication.Helper;

namespace Vani.Comminication.Service
{


    public class InMemoryRateLimiterService : IRateLimiterService
    {
        private readonly int _maxPerNumberPerSecond;
        private readonly int _maxPerAccountPerSecond;
        private readonly ConcurrentDictionary<string, SlidingWindowCounter> _numberLimits = new();
        private readonly SlidingWindowCounter _accountLimit;

        public InMemoryRateLimiterService(IConfiguration configuration)
        {
            var messageRateLimits = new MessageRateLimitsConfig();
            configuration.GetSection(MessageRateLimitsConfig.CONFIG_SECTION_TITLE).Bind(messageRateLimits);

            _maxPerNumberPerSecond = messageRateLimits.PhoneNumberRateLimitPerSecond;
            _maxPerAccountPerSecond = messageRateLimits.AccountRateLimitIPerSecond;

            _accountLimit = new SlidingWindowCounter(_maxPerAccountPerSecond);
        }

        public async Task<bool> CanSendFromNumber(string phoneNumber)
        {
            var numberCounter = _numberLimits.GetOrAdd(phoneNumber, _ => new SlidingWindowCounter(_maxPerNumberPerSecond));
            var result = numberCounter.Increment() && _accountLimit.Increment();
            var isSuccessful = Task.FromResult(result);
            // Add a call to insert a record into the RateLimitServiceAuditLog 
            return await Task.FromResult(result);
        }

        public void CleanupInactiveNumbers(TimeSpan inactiveThreshold)
        {
            foreach (var kvp in _numberLimits)
            {
                if (kvp.Value.LastAccessed < DateTime.UtcNow - inactiveThreshold)
                    _numberLimits.TryRemove(kvp.Key, out _);
            }
        }
    }

}
