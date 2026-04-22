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
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 10;
        if (page < 1) page = 1;

        var totalCount = await _context.Transactions.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
        if (page > totalPages) page = totalPages;

        var transactions = await _context.Transactions
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Decrypt the IBAN for display
        foreach (var t in transactions)
            t.EncryptedIban = _encryptionService.Decrypt(t.EncryptedIban);

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;

        return View(transactions);
    }
}
