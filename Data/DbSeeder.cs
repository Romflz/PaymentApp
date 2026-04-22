using Microsoft.EntityFrameworkCore;
using PaymentApp.Models;

namespace PaymentApp.Data;

public static class DbSeeder
{
    public static async Task SeedBlacklistAsync(AppDbContext db)
    {
        if (await db.BlacklistedPersons.AnyAsync()) return;

        var names = new[]
        {
            "John Doe",
            "Jane Smith",
            "Viktor Petrov",
            "Mario Rossi",
            "Ahmed Khan",
            "Sofia Martinez",
            "Hans Müller",
            "Chen Wei"
        };

        db.BlacklistedPersons.AddRange(names.Select(n => new BlacklistedPerson { Name = n }));
        await db.SaveChangesAsync();
    }
}
