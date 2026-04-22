using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentApp.Data;

namespace PaymentApp.Controllers;

public class BlockedController : Controller
{
    private readonly AppDbContext _context;

    public BlockedController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("/blocked")]
    public async Task<IActionResult> Index()
    {
        var blocked = await _context.BlacklistedPersons
            .OrderBy(b => b.Name)
            .ToListAsync();

        return View(blocked);
    }
}
