using BudgetBuddy.Model;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Data;

public class BudgetBuddyContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<GoalModel> GoalModel { get; set; }

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
    }
}