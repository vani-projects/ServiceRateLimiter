namespace Vani.Comminication.Contracts
{
    public interface IRateLimiter
    {
        Task<bool> CanSendFromNumber(string phoneNumber);
    }
}
