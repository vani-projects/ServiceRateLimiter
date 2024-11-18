using Vani.Comminication.Contracts;

namespace Vani.Comminication.Repositories
{
    public class MessageRateLimits : IMessageRateLimits
    {        
        public int PhoneNumberRateLimit { get; set; }
        public int AccountRateLimit { get;set; }
       
    }
}
