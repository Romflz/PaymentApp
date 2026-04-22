using Microsoft.EntityFrameworkCore;
using PaymentApp.Models;

namespace PaymentApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<BlacklistedPerson> BlacklistedPersons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set Amount to 18 digits total, 2 decimal places (e.g. 9999999999999999.99)
        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);
    }
}