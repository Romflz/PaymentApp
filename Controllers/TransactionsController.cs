using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentApp.Data;
using PaymentApp.Services;

namespace PaymentApp.Controllers;

public class TransactionsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IEncryptionService _encryptionService;

    public TransactionsController(AppDbContext context, IEncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    [HttpGet("/transactions")]
    public async Task<IActionResult> Index()
    {
        var transactions = await _context.Transactions
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        // Decrypt the IBAN for display
        foreach (var t in transactions)
            t.EncryptedIban = _encryptionService.Decrypt(t.EncryptedIban);

        return View(transactions);
    }
}
