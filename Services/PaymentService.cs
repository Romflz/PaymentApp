using PaymentApp.Data;
using PaymentApp.Models;

namespace PaymentApp.Services;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _context;
    private readonly IValidationService _validationService;
    private readonly IBlacklistService _blacklistService;

    public PaymentService(AppDbContext context, IValidationService validationService, IBlacklistService blacklistService)
    {
        _context = context;
        _validationService = validationService;
        _blacklistService = blacklistService;
    }

    public async Task<Transaction> ProcessPayment(string senderName, decimal amount, string iban)
    {
        var transaction = new Transaction
        {
            SenderName = senderName,
            Amount = amount,
            EncryptedIban = iban,
            CreatedAt = DateTime.UtcNow
        };

        var errors = new List<string>();

        if (!_validationService.IsValidName(senderName))
            errors.Add("Sender name is required.");

        if (!_validationService.IsValidAmount(amount))
            errors.Add("Amount must be greater than zero.");

        var ibanError = _validationService.GetIbanError(iban);
        if (ibanError != null)
            errors.Add(ibanError);

        if (_validationService.IsValidName(senderName) && await _blacklistService.IsBlacklisted(senderName))
            errors.Add("Sender is blacklisted.");

        if (errors.Count > 0)
        {
            transaction.Result = "Rejected";
            transaction.RejectionReason = string.Join(" | ", errors);
        }
        else
        {
            transaction.Result = "Accepted";
            transaction.TransactionReference = Guid.NewGuid().ToString().ToUpper();
        }

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }
}
