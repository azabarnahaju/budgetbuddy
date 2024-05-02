using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.TransactionEnums;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BudgetBuddy.Data;

public class BudgetBuddyContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Report> Reports { get; set; }


    public BudgetBuddyContext(DbContextOptions<BudgetBuddyContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Achievement>()
            .HasIndex(a => a.Name)
            .IsUnique();
        
        modelBuilder.Entity<Achievement>()
            .HasMany(e => e.Users)
            .WithMany(e => e.Achievements);
        
        modelBuilder.Entity<Account>()
            .HasMany(e => e.Transactions)
            .WithOne(e => e.Account)
            .HasForeignKey(e => e.AccountId)
            .IsRequired();

        modelBuilder.Entity<Account>()
            .HasOne(a => a.User)
            .WithMany(a => a.Accounts)
            .HasForeignKey(a => a.UserId)
            .IsRequired();
        
        modelBuilder.Entity<Goal>()
            .HasOne(g => g.Account)
            .WithMany(a => a.Goals)
            .HasForeignKey(g => g.AccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        modelBuilder.Entity<Report>()
            .HasOne(r => r.Account)
            .WithMany(a => a.Reports)
            .HasForeignKey(r => r.AccountId)
            .IsRequired();

        modelBuilder.Entity<Report>()
            .HasMany(r => r.Transactions)
            .WithMany(t => t.Reports)
            .UsingEntity<Dictionary<string, object>>(
                "ReportTransaction",
                // Configure join table
                r => r.HasOne<Transaction>().WithMany().HasForeignKey("TransactionId")
                    .OnDelete(DeleteBehavior.Restrict),
                t => t.HasOne<Report>().WithMany().HasForeignKey("ReportId").OnDelete(DeleteBehavior.Restrict),
                // Configure additional properties if needed
                jt =>
                {
                    jt.HasKey("ReportId", "TransactionId");
                    // Configure additional properties of the join table if needed
                });
        
        modelBuilder.Entity<Report>()
            .Property(e => e.Categories)
            .HasConversion(
                v => string.Join(',', v),
                v => new HashSet<TransactionCategoryTag>(v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(e => (TransactionCategoryTag)Enum.Parse(typeof(TransactionCategoryTag), e)))
                , new ValueComparer<HashSet<TransactionCategoryTag>>(
                     (c1, c2) => c1.SequenceEqual(c2),
                     c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                     c => new HashSet<TransactionCategoryTag>(c))
            );
        
        modelBuilder.Entity<Report>()
            .Property(e => e.SpendingByTags)
            .HasConversion(
                v => string.Join(';', v.Select(kvp => $"{(int)kvp.Key}:{kvp.Value:F2}")),
                v => v.Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Select(pair => pair.Split(':', StringSplitOptions.None))
                    .ToDictionary(
                        pair => (TransactionCategoryTag)int.Parse(pair[0]),
                        pair => decimal.Parse(pair[1])
                    )
                ,
                new ValueComparer<Dictionary<TransactionCategoryTag, decimal>>(
                    (d1, d2) => d1.SequenceEqual(d2),
                    d => d.Aggregate(0, (a, kvp) => HashCode.Combine(a, kvp.Key.GetHashCode(), kvp.Value.GetHashCode())),
                    d => d.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
            );
        
        modelBuilder.Entity<Report>()
            .Property(e => e.MostSpendingTag)
            .HasConversion<int>();
    }
}