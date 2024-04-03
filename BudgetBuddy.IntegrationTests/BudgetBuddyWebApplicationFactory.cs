using System.Data.Common;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            
            var sp = services.BuildServiceProvider();

            // using (var scope = sp.CreateScope())
            // {
            //     var scopedServices = scope.ServiceProvider;
            //     var db = scopedServices.GetRequiredService<BudgetBuddyContext>();
            //     var logger = scopedServices.GetRequiredService<ILogger<BudgetBuddyWebApplicationFactory<TProgram>>>();
            //
            //     db.Database.EnsureDeleted();
            //     db.Database.EnsureCreated();
            //
            //     try
            //     {
            //         Console.WriteLine("Seeding...");
            //         SeedData.Initialize(db);
            //         Console.WriteLine($"Data seeded");
            //     }
            //     catch (Exception ex)
            //     {
            //         logger.LogError(ex, "An error occurred seeding the " + "database with test messages. Error: {Message}", ex.Message);
            //     }
            // }
            
            services.AddScoped<AuthenticationSeeder>(provider =>
            {
                var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            
                // Fetch adminInfo from configuration or any other source
                var adminInfo = new Dictionary<string, string>
                {
                    {"adminEmail", "admin@test.com"},
                    {"adminPassword", "adminTESTpassword"}
                };

                return new AuthenticationSeeder(roleManager, userManager, adminInfo);
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
            
            SeedTestData(services);
        });
        builder.UseEnvironment("Development");
    }
    
    void SeedTestData(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<BudgetBuddyContext>();
        
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Users.Add(new ApplicationUser { Id = "1", UserName = "User1", Email = "testuser1@email.com" });
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