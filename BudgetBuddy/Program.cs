using BudgetBuddy.Model;
using BudgetBuddy.Services.Authentication;
using BudgetBuddy.Services.Repositories.Account;
using BudgetBuddy.Services.Repositories.Achievement;
using BudgetBuddy.Services.Repositories.User;
using BudgetBuddy.Services.Repositories.Transaction;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Net.Http.Headers;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

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
builder.Services.AddSingleton<IAccountRepository>(provider => new AccountRepository(new List<Account>()));

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

builder.Services.AddCors(options =>  
    options.AddPolicy("Development", builder =>  
    {  
        // Allow multiple HTTP methods  
        builder.WithMethods("GET", "POST", "PATCH", "DELETE", "OPTIONS")  
            .WithHeaders(  
                HeaderNames.Accept,  
                HeaderNames.ContentType,  
                HeaderNames.Authorization)  
            .AllowCredentials()  
            .SetIsOriginAllowed(origin =>  
            {  
                if (string.IsNullOrWhiteSpace(origin)) return false;  
                if (origin.ToLower().StartsWith("http://localhost")) return true;  
                return false;  
            });  
    })  
);  

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var connection ="http://localhost:5173/";
app.UseCors(b => {
    b.WithOrigins(connection!)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithExposedHeaders("content-type") // Allow the 'content-type' header to be exposed
        .SetIsOriginAllowed(_ => true); // Allow any origin for CORS
});

app.UseCookiePolicy(  
    new CookiePolicyOptions  
    {  
        Secure = CookieSecurePolicy.Always  
    });  

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run("https://localhost:7204");