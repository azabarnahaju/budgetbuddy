using BudgetBuddy.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Applying migrations");
        
// Env.Load();
// var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

var optionsBuilder = new DbContextOptionsBuilder<BudgetBuddyContext>();
optionsBuilder.UseSqlServer("Data Source=host.docker.internal,1433;Database=BudgetBuddy;User Id=sa;Password=Bogar2006.;Encrypt=false;TrustServerCertificate=true;");

using (var context = new BudgetBuddyContext(optionsBuilder.Options))
{
    context.Database.Migrate();
}

Console.WriteLine("Done");