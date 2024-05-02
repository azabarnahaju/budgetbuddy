namespace BudgetBuddy.Services.Repositories.User;

using Data;
using Model;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly BudgetBuddyContext _database;

    public UserRepository(BudgetBuddyContext database)
    {
        _database = database;
    }

    public async Task<ApplicationUser?> GetUserById(string id)
    {
        return await _database.Users
            .Include(u => u.Accounts)
                .ThenInclude(a => a.Transactions)
            .Include(u => u.Accounts)
                .ThenInclude(a => a.Reports)
            .Include(u => u.Accounts)
                .ThenInclude(a => a.Goals)
            .Include(applicationUser => applicationUser.Achievements)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<ApplicationUser?> GetUserByAccountId(int accountId)
    {
        return await _database.Users
            .Include(u => u.Accounts)
                .ThenInclude(a => a.Transactions)
            .Include(u => u.Accounts)
                .ThenInclude(a => a.Reports)
            .Include(u => u.Accounts)
                .ThenInclude(a => a.Goals)
            .Include(applicationUser => applicationUser.Achievements)
            .FirstOrDefaultAsync(u => u.Accounts.Any(a => a.Id == accountId));
    }

    public async Task AddAchievementToUser(string userId, Achievement achievement)
    {
        var user = await GetUserById(userId);
        if (user is null)
            throw new Exception("User not found.");
        
        user.Achievements.Add(achievement);
        await _database.SaveChangesAsync();
    }
}