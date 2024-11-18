namespace Vani.Comminication.Contracts
{
    public interface IMessageRateLimits
    {
        static string CONFIG_SECTION_TITLE = "MessageRateLimits";
        int PhoneNumberRateLimit { get; set; }
        int AccountRateLimit { get; set; }
    }
}
