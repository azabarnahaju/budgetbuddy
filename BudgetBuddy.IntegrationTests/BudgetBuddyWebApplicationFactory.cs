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
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace BudgetBuddy.IntegrationTests;

public class BudgetBuddyWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
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
                options.UseInMemoryDatabase("BudgetBuddy_Test");
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
        context.Achievements.Add(new Achievement() { Id = 1, Description = "Test", Name = "Test" });
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
        await context.SaveChangesAsync();
    }
}