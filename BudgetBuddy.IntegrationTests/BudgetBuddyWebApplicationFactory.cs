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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace BudgetBuddy.IntegrationTests;

public class BudgetBuddyWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var fakeConfiguration = new List<KeyValuePair<string, string?>>
        {
            new ("JwtSettings:ValidIssuer", "your_fake_valid_issuer"),
            new ("JwtSettings:ValidAudience", "your_fake_valid_audience"),
            new ("JwtSettings:IssuerSigningKey", "This_is_a_super_secure_key_and_you_know_it"),
            new ("AdminInfo:AdminEmail", "test@admin.com"),
            new ("AdminInfo:AdminPassword", "test123")
        };
        
        builder.ConfigureServices(services =>
        {
            var configurationServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IConfiguration));
            if (configurationServiceDescriptor != null)
            {
                services.Remove(configurationServiceDescriptor);
            }

            // Add a fake configuration provider
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(fakeConfiguration)
                .Build());
            
            // adding in-memory database
            var dbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BudgetBuddyContext>));
            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
            services.Remove(dbConnectionDescriptor);
            services.Remove(dbContextDescriptor);
            services.AddDbContext<BudgetBuddyContext>(options =>
            {
                options.UseInMemoryDatabase("BudgetBuddy_Test");
            }, ServiceLifetime.Singleton);
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
    
    void SeedTestData(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<BudgetBuddyContext>();
        context.Accounts.Add(new Account()
            { Id = 1, UserId = "1", Date = DateTime.Now, Balance = 1500, Name = "Test", Type = "Test" });
        context.Reports.Add(new Report
        {
            Id = 1, AccountId = 1, Categories = new HashSet<TransactionCategoryTag>(),
            SpendingByTags = new Dictionary<TransactionCategoryTag, decimal>()
        });
        context.Transactions.Add(new Transaction()
        {
            Id = 1, AccountId = 1, Amount = 1400, Date = DateTime.Now, Name = "Test",
            Tag = TransactionCategoryTag.Clothing, Type = TransactionType.Expense
        });
        context.Achievements.Add(new Achievement() { Id = 1, Description = "Test", Name = "Test" });
        context.Goals.Add(new Goal()
        {
            AccountId = 1, UserId = "1", Id = 1, Completed = false, CurrentProgress = 0, StartDate = DateTime.Now,
            Type = GoalType.Income, Target = 100
        });
        context.SaveChanges();
    }
}