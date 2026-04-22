namespace PaymentApp.Models;

public class Transaction
{
    public int Id { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string EncryptedIban { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
    public string? TransactionReference { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}