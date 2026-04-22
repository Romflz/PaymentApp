namespace PaymentApp.Services;

public interface IValidationService
{
    bool IsValidName(string name);
    bool IsValidAmount(decimal amount);
    bool IsValidIban(string iban);
    string? GetIbanError(string iban);
}