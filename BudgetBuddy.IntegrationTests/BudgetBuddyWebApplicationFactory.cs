using System.Data.Common;
using System.Text;
using BudgetBuddy.Data;
using BudgetBuddy.IntegrationTests.JwtAuthenticationTest;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace BudgetBuddy.IntegrationTests;

public class BudgetBuddyWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var fakeConfiguration = new List<KeyValuePair<string, string?>>
        {
            new ("ValidIssuer", "your_fake_valid_issuer"),
            new ("ValidAudience", "your_fake_valid_audience"),
            new ("IssuerSigningKey", "This_is_a_super_secure_key_and_you_know_it"),
            new ("AdminEmail", "test@admin.com"),
            new ("AdminPassword", "test123")
        };
        
        builder.ConfigureServices(services =>
        {
            var configurationServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IConfiguration));
            if (configurationServiceDescriptor != null)
            {
                services.Remove(configurationServiceDescriptor);
            }

            services.Remove(services.SingleOrDefault(service => service.ServiceType == typeof(JwtBearerOptions)));
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
                        Issuer = "your_fake_valid_issuer",
                    };
                    options.TokenValidationParameters.ValidIssuer = "your_fake_valid_issuer";
                    options.TokenValidationParameters.ValidAudience = "your_fake_valid_audience";
                    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This_is_a_super_secure_key_and_you_know_it"));
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
        context.Users.Add(new ApplicationUser() { UserName = "User", Email = "test@email.com" });
        context.Accounts.Add(new Account()
            { UserId = "1", Date = DateTime.Now, Balance = 1500, Name = "Test", Type = "Test" });
        context.Reports.Add(new Report
        {
            AccountId = 1, Categories = new HashSet<TransactionCategoryTag>(),
            SpendingByTags = new Dictionary<TransactionCategoryTag, decimal>()
        });
        context.Transactions.Add(new Transaction()
        {
            AccountId = 1, Amount = 1400, Date = DateTime.Now, Name = "Test",
            Tag = TransactionCategoryTag.Clothing, Type = TransactionType.Expense
        });
        context.Achievements.Add(new Achievement() { Description = "Test", Name = "Test" });
        context.Goals.Add(new Goal()
        {
            AccountId = 1, UserId = "1", Completed = false, CurrentProgress = 0, StartDate = DateTime.Now,
            Type = GoalType.Income, Target = 100
        });
        context.SaveChanges();
    }
}