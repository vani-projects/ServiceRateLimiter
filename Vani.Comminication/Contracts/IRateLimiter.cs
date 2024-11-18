namespace Vani.Comminication.Contracts
{
    public interface IRateLimiter
    {
        Task<bool> CanSendFromNumber(string phoneNumber);
        void CleanupInactiveNumbers(TimeSpan inactiveThreshold);
    }
}
