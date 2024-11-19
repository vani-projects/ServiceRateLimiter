using System.Runtime.ConstrainedExecution;

namespace Vani.Comminication.Config
{
    public class MessageRateLimitsConfig
    {
        public const string CONFIG_SECTION_TITLE = "MessageRateLimits";
        public int PhoneNumberRateLimitPerSecond { get; set; }
        public int AccountRateLimitIPerSecond { get; set; }

    }
}
