using Microsoft.EntityFrameworkCore;
using PaymentApp.Data;

namespace PaymentApp.Services;

public class BlacklistService : IBlacklistService
{
    private readonly AppDbContext _context;

    public BlacklistService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsBlacklisted(string name)
    {
        return await _context.BlacklistedPersons
            .AnyAsync(b => b.Name.ToLower() == name.ToLower());
    }
}