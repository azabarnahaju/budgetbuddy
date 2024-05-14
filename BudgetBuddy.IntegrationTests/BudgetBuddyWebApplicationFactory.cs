using System.Data.Common;
using BudgetBuddy.Data;
using BudgetBuddy.IntegrationTests.JwtAuthenticationTest;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.AchievementEnums;
using BudgetBuddy.Model.Enums.TransactionEnums;
using BudgetBuddy.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace BudgetBuddy.IntegrationTests;

public class BudgetBuddyWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly string _dbName = Guid.NewGuid().ToString();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "test");
        builder.ConfigureServices(services =>
        {
            // adding in-memory database
            var dbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BudgetBuddyContext>));
            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
            var authSeeder = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthenticationSeeder));
            services.Remove(dbConnectionDescriptor);
            services.Remove(dbContextDescriptor);
            services.Remove(authSeeder);
            services.AddDbContext<BudgetBuddyContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            }, ServiceLifetime.Singleton);
            services.AddScoped<IAuthenticationSeeder, FakeAuthenticationSeeder>();
            // adding JWT authorization
            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.Configuration = new OpenIdConnectConfiguration
                    {
                        Issuer = JwtTokenProvider.Issuer,
                    };
                    options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                    options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                    options.TokenValidationParameters.IssuerSigningKey = JwtTokenProvider.SecurityKey;
                }
            );
            
            SeedTestData(services);
        });
    }
    
    async Task SeedTestData(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<BudgetBuddyContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        
        context.Accounts.Add(new Account()
            { Id = 1, UserId = "1", Date = new DateTime(2022, 02, 02), Balance = 1500, Name = "Test", Type = "Test" });
        context.Transactions.Add(new Transaction()
        {
            Id = 1, AccountId = 1, Amount = 1400, Date = new DateTime(2022, 02, 02), Name = "Test",
            Tag = TransactionCategoryTag.Clothing, Type = TransactionType.Expense
        });
        context.Achievements.AddRange(new List<Achievement>
            {
                new Achievement("Pioneer", AchievementType.Exploration, 1, AchievementObjectiveType.Transaction),
                new Achievement("Money Tracker", AchievementType.Exploration, 10, AchievementObjectiveType.Transaction),
                new Achievement("Transaction Pro", AchievementType.Exploration, 25, AchievementObjectiveType.Transaction),
                new Achievement("Account Starter", AchievementType.Exploration, 1, AchievementObjectiveType.Account),
                new Achievement("Multi-Account Holder", AchievementType.Exploration, 3, AchievementObjectiveType.Account),
                new Achievement("Master of Accounts", AchievementType.Exploration, 5, AchievementObjectiveType.Account),
                new Achievement("Income Beginner", AchievementType.Exploration, 1, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Income Earner", AchievementType.Exploration, 10, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Income Guru", AchievementType.Exploration, 25, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Master of Income", AchievementType.Exploration, 50, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Expense Beginner", AchievementType.Exploration, 1, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Expense Proficient", AchievementType.Exploration, 10, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Big Spender", AchievementType.Exploration, 25, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Master of Expenses", AchievementType.Exploration, 50, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement ("Goal Getter", AchievementType.Exploration, 1, AchievementObjectiveType.Goal),
                new Achievement ("Goal Digger", AchievementType.Exploration, 3, AchievementObjectiveType.Goal),
                new Achievement ("Goal Crusher", AchievementType.Exploration, 5, AchievementObjectiveType.Goal),
                new Achievement("Penny Pincher", AchievementType.AmountBased, 100, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Budget Boss", AchievementType.AmountBased, 500, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Financial Guru", AchievementType.AmountBased, 2000, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Money Master", AchievementType.AmountBased, 5000, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Money Maker", AchievementType.AmountBased, 100, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Cash Flow Captain", AchievementType.AmountBased, 500, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Income Innovator", AchievementType.AmountBased, 2000, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Wealth Wizard", AchievementType.AmountBased, 5000, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Entertainment Explorer", AchievementType.AmountBased, 50, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment),
                new Achievement("Leisure Luminary", AchievementType.AmountBased, 150, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment),
                new Achievement("Fun Fund Fancier", AchievementType.AmountBased, 500, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment),
                new Achievement("Entertainment Enthusiast", AchievementType.AmountBased, 1000, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment),
            });
        context.Goals.Add(new Goal()
        {
            AccountId = 1, UserId = "1", Id = 1, Completed = false, CurrentProgress = 0, StartDate = new DateTime(2022, 02, 02),
            Type = GoalType.Income, Target = 100
        });
        context.Reports.Add(new Report()
        {
            Id = 1, AccountId = 1, ReportType = ReportType.Monthly, StartDate = new DateTime(2022, 02, 02),
            EndDate = new DateTime(2022, 02, 02), SumExpense = 100, AverageSpendingDaily = 100,
            AverageSpendingTransaction = 100, MostSpendingTag = TransactionCategoryTag.Education,
            MostSpendingDay = new DateTime(2022, 02, 02), BiggestExpense = 100, CreatedAt = new DateTime(2022, 02, 02),
            SumIncome = 100, Categories = new HashSet<TransactionCategoryTag>(),
            SpendingByTags = new Dictionary<TransactionCategoryTag, decimal>()
        });
        context.Users.Add(new ApplicationUser { Id = "1", Email = "test@test.com", UserName = "testuser" });
        await context.SaveChangesAsync();
    }
}