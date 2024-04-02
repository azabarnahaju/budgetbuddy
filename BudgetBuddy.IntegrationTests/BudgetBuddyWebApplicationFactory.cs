using System.Data.Common;
using BudgetBuddy.Data;
using BudgetBuddy.IntegrationTests.JwtAuthenticationTest;
using BudgetBuddy.Model;
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
        builder.ConfigureServices(services =>
        {
            // adding in-memory database
            var dbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BudgetBuddyContext>));
            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
            services.Remove(dbConnectionDescriptor);
            services.Remove(dbContextDescriptor);

            services.AddDbContext<BudgetBuddyContext>(options =>
            {
                options.UseInMemoryDatabase("BudgetBuddy_Test");
            });
            
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
            
            // SeedTestData(services);
        });
    }
    
    // void SeedTestData(IServiceCollection services)
    // {
    //     using var scope = services.BuildServiceProvider().CreateScope();
    //     var serviceProvider = scope.ServiceProvider;
    //     var context = serviceProvider.GetRequiredService<BudgetBuddyContext>();
    //
    //     context.Users.Add(new ApplicationUser { Id = "testUserId", Email = "user1@user1.com", UserName = "user1", Accounts = new List<Account> { new Account { Id = 1, Date = new DateTime(2024, 04, 02), Balance = 150, Name = "Test1", UserId = "1", Type = "Test"} }, Achievements = new List<Achievement> { new Achievement { Id = 5 } }});
    //     context.Accounts.Add(new Account { Id = 1, Date = new DateTime(2024, 04, 02), Balance = 150, Name = "Test1", UserId = "1", Type = "Test", Reports = new List<Report> { new Report { Id = 3, AccountId = 1 } }});
    //     context.Goals.Add(new Goal { Id = 2, UserId = "testUserId", AccountId = 1 });
    //     context.Reports.Add(new Report { Id = 3, AccountId = 1 });
    //     context.Transactions.Add(new Transaction { Id = 4, AccountId = 1 });
    //     context.Achievements.Add(new Achievement { Id = 5, Users = new List<ApplicationUser> { new ApplicationUser { Id = "testUserId", Email = "user1@user1.com", UserName = "user1", Accounts = new List<Account> { new Account { Id = 1, Date = new DateTime(2024, 04, 02), Balance = 150, Name = "Test1", UserId = "1", Type = "Test"} }, Achievements = new List<Achievement> { new Achievement { Id = 5 } }}}});
    //     context.SaveChanges();
    // }
}