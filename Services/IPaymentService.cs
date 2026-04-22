using PaymentApp.Models;

namespace PaymentApp.Services;

public interface IPaymentService
{
    Task<Transaction> ProcessPayment(string senderName, decimal amount, string iban);
}