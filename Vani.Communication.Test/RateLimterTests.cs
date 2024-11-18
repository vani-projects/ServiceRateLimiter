using Vani.Comminication.Helper;

namespace Vani.Communication.Test
{
    public class RateLimterTests  
    {
        [Fact]
        public async Task CanSendFromNumberOverLimitsReturnsFalse()
        {
            var rateLimiter = new InMemoryRateLimiter(1, 2);
            await rateLimiter.CanSendFromNumber("1234567890");
            var result = await rateLimiter.CanSendFromNumber("1234567890");
            Assert.False(result);
        }

        [Fact]
        public async Task CanSendFromNumberInLimitsReturnsTrue()
        {
            var rateLimiter = new InMemoryRateLimiter(1, 2);            
            Assert.True(await rateLimiter.CanSendFromNumber("1234567890"));
        }

        [Fact]
        public async Task CanSendFromAccountOverLimitsReturnsFalse()
        {
            var rateLimiter = new InMemoryRateLimiter(1, 2);
            await rateLimiter.CanSendFromNumber("1234567890");
            await rateLimiter.CanSendFromNumber("0987654321");
            var result = await rateLimiter.CanSendFromNumber("5555555555");
            Assert.False(result);
        }

        [Fact]
        public async Task CanSendFromAccountInLimitsReturnsTrue()
        {
            var rateLimiter = new InMemoryRateLimiter(1, 2);
            await rateLimiter.CanSendFromNumber("0987654321");
            Assert.True(await rateLimiter.CanSendFromNumber("1234567890"));
        }

        [Fact]
        public async Task CanSendFromPhoneOverLimitAfterASecondTrue()
        {
            var rateLimiter = new InMemoryRateLimiter(1, 2);
            await rateLimiter.CanSendFromNumber("1234567890");
            Thread.Sleep(1000);
            var result = await rateLimiter.CanSendFromNumber("1234567890");
            Assert.True(result);
        }
    }
}