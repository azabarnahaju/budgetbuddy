using BudgetBuddy.Data;
using BudgetBuddy.Model;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly BudgetBuddyContext _database;

    public UserRepository(BudgetBuddyContext database)
    {
        _database = database;
    }

    public async Task<ApplicationUser?> GetUserById(string id)
    {
        var user = await _database.Users.FirstOrDefaultAsync(u => u.Id == id);
        Console.WriteLine(user.Id);
        return user;
    }

    public async Task<ApplicationUser?> GetUserByAccountId(int accountId)
    {
        return await _database.Users.FirstOrDefaultAsync(u => u.Accounts.Any(a => a.Id == accountId));
    }
}