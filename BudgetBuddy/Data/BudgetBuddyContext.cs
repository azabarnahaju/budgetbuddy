using BudgetBuddy.Model;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Data;

public class BudgetBuddyContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    // private readonly string _connectionString;

    // public BudgetBuddyContext(string connectionString)
    // {
    //     Env.Load();
    //     _connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    //
    //     if (string.IsNullOrEmpty(_connectionString))
    //     {
    //         throw new InvalidOperationException("DB_CONNECTION_STRING is not defined in the .env file.");
    //     }
    // }

    public BudgetBuddyContext(DbContextOptions<BudgetBuddyContext> options) : base(options)
    {
    }
    
    
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(_connectionString);
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<Achievement>()
            .HasIndex(a => a.Name)
            .IsUnique();
        
        modelBuilder.Entity<Account>()
            .HasMany(e => e.Transactions)
            .WithOne(e => e.Account)
            .HasForeignKey(e => e.AccountId)
            .IsRequired();

        modelBuilder.Entity<Achievement>()
            .HasMany(e => e.Users)
            .WithMany(e => e.Achievements)
            .UsingEntity(
                "UserAchievement",
                l => l.HasOne(typeof(User)).WithMany().HasForeignKey("UsersId").HasPrincipalKey(nameof(User.Id)),
                r => r.HasOne(typeof(Achievement)).WithMany().HasForeignKey("AchievementsId")
                    .HasPrincipalKey(nameof(Achievement.Id)),
                j => j.HasKey("AchievementsId", "UsersId"));
    }
}