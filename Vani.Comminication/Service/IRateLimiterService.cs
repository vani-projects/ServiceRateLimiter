namespace Vani.Comminication.Service
{
    public interface IRateLimiterService
    {
        Task<bool> CanSendFromNumber(string phoneNumber);
        void CleanupInactiveNumbers(TimeSpan inactiveThreshold);
    }
}
