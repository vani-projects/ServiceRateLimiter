using Microsoft.Extensions.Configuration;
using Vani.Comminication.Service;

namespace Vani.Communication.Test
{
    public class RateLimterTests  
    {
        IConfiguration _configuration;
        public RateLimterTests() { 
            var appsettingConfig = new Dictionary<string, string>
            {
                {"RateLimitsResourceExpiryInSeconds", "60"},
                {"MessageRateLimits:AccountRateLimitIPerSecond", "2"},
                {"MessageRateLimits:PhoneNumberRateLimitPerSecond", "1"},
            };
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(appsettingConfig).Build();
        }

        [Fact]
        public async Task CanSendFromNumberOverLimitsReturnsFalse()
        {
            var rateLimiter = new InMemoryRateLimiterService(_configuration);

            await rateLimiter.CanSendFromNumber("1234567890");
            var result = await rateLimiter.CanSendFromNumber("1234567890");
            Assert.False(result);
        }

        [Fact]
        public async Task CanSendFromNumberInLimitsReturnsTrue()
        {
            var rateLimiter = new InMemoryRateLimiterService(_configuration);            
            Assert.True(await rateLimiter.CanSendFromNumber("1234567890"));
        }

        [Fact]
        public async Task CanSendFromAccountOverLimitsReturnsFalse()
        {
            var rateLimiter = new InMemoryRateLimiterService(_configuration);
            await rateLimiter.CanSendFromNumber("1234567890");
            await rateLimiter.CanSendFromNumber("0987654321");
            var result = await rateLimiter.CanSendFromNumber("5555555555");
            Assert.False(result);
        }

        [Fact]
        public async Task CanSendFromAccountInLimitsReturnsTrue()
        {
            var rateLimiter = new InMemoryRateLimiterService(_configuration);
            await rateLimiter.CanSendFromNumber("0987654321");
            Assert.True(await rateLimiter.CanSendFromNumber("1234567890"));
        }

        [Fact]
        public async Task CanSendFromPhoneOverLimitAfterASecondTrue()
        {
            var rateLimiter = new InMemoryRateLimiterService(_configuration);
            await rateLimiter.CanSendFromNumber("1234567890");
            Thread.Sleep(1000);
            var result = await rateLimiter.CanSendFromNumber("1234567890");
            Assert.True(result);
        }
    }
}