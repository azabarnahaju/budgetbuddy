using BudgetBuddy.Services.Authentication;
using BudgetBuddy.Services.Repositories.Account;
using BudgetBuddy.Services.Repositories.Achievement;
using BudgetBuddy.Services.Repositories.User;
using BudgetBuddy.Services.Repositories.Transaction;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<IAchievementRepository, AchievementRepository>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddAuthentication(options => { 
    options.DefaultScheme = "Cookies"; 
}).AddCookie("Cookies", options => {
    options.Cookie.Name = "Cookie_Name";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Events = new CookieAuthenticationEvents
    {                          
        OnRedirectToLogin = redirectContext =>
        {
            redirectContext.HttpContext.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
    };                
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCookiePolicy(  
    new CookiePolicyOptions  
    {  
        Secure = CookieSecurePolicy.Always  
    });  

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();