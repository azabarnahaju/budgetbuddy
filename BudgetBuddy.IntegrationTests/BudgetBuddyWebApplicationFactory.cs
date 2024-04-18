using System.Data.Common;
using BudgetBuddy.Data;
using BudgetBuddy.IntegrationTests.JwtAuthenticationTest;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
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
                new Achievement
                    { Name = "Pioneer", Description = "You've recorded your first expense transaction!" },
                new Achievement
                    { Name = "Big Spender", Description = "You've recorded 5 expense transactions!" },
                new Achievement
                    { Name = "Money Bags", Description = "You've recorded 10 expense transactions!" },

                new Achievement
                    { Name = "Money Maker", Description = "You've recorded your first income transaction!" },
                new Achievement
                    { Name = "Wealth Builder", Description = "You've recorded 5 income transactions!" },
                new Achievement
                    { Name = "Financial Wizard", Description = "You've recorded 10 income transactions!" },

                new Achievement { Name = "Account Holder", Description = "You've created your first account!" },

                new Achievement
                    { Name = "Penny Pincher", Description = "You've saved up $500 in your account!" },
                new Achievement { Name = "Frugal", Description = "You've saved up $1000 in your account!" },
                new Achievement { Name = "Thrifty", Description = "You've saved up $1500 in your account!" },

                new Achievement { Name = "Goal Setter", Description = "You've set your first goal!" },
                new Achievement { Name = "Goal Achiever", Description = "You've set 3 goals!" },
                new Achievement { Name = "Master of Goals", Description = "You've set 5 goals!" },
                
                new Achievement { Name = "Goal Getter", Description = "You've completed your first goal!" },
                new Achievement { Name = "Goal Digger", Description = "You've completed 3 goals!" },
                new Achievement { Name = "Goal Crusher", Description = "You've completed 5 goals!" },

                new Achievement
                    { Name = "Five-Star Dabbler", Description = "You've used 5 different categories!" },
                new Achievement { Name = "Jack of All Trades", Description = "You've used 10 categories!" },
                new Achievement { Name = "Master of All", Description = "You've used ALL categories!" }
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