namespace PaymentApp.Services;

public interface IBlacklistService
{
    Task<bool> IsBlacklisted(string name);
}