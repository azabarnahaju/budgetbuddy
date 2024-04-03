﻿using System.Data.Common;
using System.Text;
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
using Microsoft.IdentityModel.Tokens;
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
            var authenticationSeeder = services.SingleOrDefault(d => d.ServiceType == typeof(IAuthenticationSeeder));
            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
            var tokenService = services.SingleOrDefault(d => d.ServiceType == typeof(ITokenService));
            services.Remove(authenticationSeeder);
            services.Remove(dbConnectionDescriptor);
            services.Remove(dbContextDescriptor);
            services.Remove(tokenService);
            services.AddDbContext<BudgetBuddyContext>(options =>
            {
                options.UseInMemoryDatabase("BudgetBuddy_Test");
            }, ServiceLifetime.Singleton);
            
            
            services.AddScoped<IAuthenticationSeeder, FakeAuthenticationSeeder>();
            // adding JWT authorization
            services.AddScoped<ITokenService>(provider =>
                new TokenService("testIssuer", "testAudience", "This_is_a_super_secure_key_and_you_know_it"));

            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.Configuration = new OpenIdConnectConfiguration
                    {
                        Issuer = "testIssuer",
                    };
                    options.TokenValidationParameters.ValidIssuer = "testIssuer";
                    options.TokenValidationParameters.ValidAudience = "testAudience";
                    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes("This_is_a_super_secure_key_and_you_know_it")
                    );
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