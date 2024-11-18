using System.Collections.Concurrent;
using Vani.Comminication.Contracts;
using Vani.Comminication.Repositories;

namespace Vani.Comminication.Helper
{
    

    public class InMemoryRateLimiter : IRateLimiter
    {
        private readonly int _maxPerNumberPerSecond;
        private readonly int _maxPerAccountPerSecond;
        private readonly ConcurrentDictionary<string, SlidingWindowCounter> _numberLimits = new();
        private readonly SlidingWindowCounter _accountLimit;

        public InMemoryRateLimiter(int maxPerNumberPerSecond, int maxPerAccountPerSecond)
        {
            _maxPerNumberPerSecond = maxPerNumberPerSecond;
            _maxPerAccountPerSecond = maxPerAccountPerSecond;
            _accountLimit = new SlidingWindowCounter(_maxPerAccountPerSecond);
        }

        public async Task<bool> CanSendFromNumber(string phoneNumber)
        {
            var numberCounter =  _numberLimits.GetOrAdd(phoneNumber, _ => new SlidingWindowCounter(_maxPerNumberPerSecond));
            var result = numberCounter.Increment() && _accountLimit.Increment();
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
