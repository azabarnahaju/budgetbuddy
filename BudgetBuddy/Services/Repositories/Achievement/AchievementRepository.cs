using BudgetBuddy.Data;
using BudgetBuddy.Model.CreateModels;
using BudgetBuddy.Model.UpdateModels;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services.Repositories.Achievement;

using Model;

public class AchievementRepository : IAchievementRepository
{
    private readonly BudgetBuddyContext _database;

    public AchievementRepository(BudgetBuddyContext database)
    {
        _database = database;
    }

    public async Task<IEnumerable<Achievement>> GetAllAchievements()
    {
        return await _database.Achievements.ToListAsync();
    }

    public async Task<Achievement> GetAchievement(int id)
    {
        return await _database.Achievements.AllAsync(a => a.Id != id)
            ? throw new Exception("Achievement could not be found.")
            : await _database.Achievements.FirstAsync(a => a.Id == id);
    }

    public async Task<Achievement> AddAchievement(AchievementInputModel achievement)
    {
        try
        {
            var achievementToCreate = new Achievement()
            {
                Name = achievement.Name,
                Description = achievement.Description
            };
            var newAchievement = await _database.Achievements.AddAsync(achievementToCreate);
            await _database.SaveChangesAsync();
        
            return newAchievement.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Cannot create new achievement.");
        }
        
    }

    public async Task DeleteAchievement(int id)
    {
        if (!await _database.Achievements.AnyAsync() || await _database.Achievements.AllAsync(a => a.Id != id)) 
            throw new Exception("Achievement is not found.");
        
        _database.Achievements.Remove(await GetAchievement(id));
        await _database.SaveChangesAsync();
    }
    
    public async Task<Achievement> UpdateAchievement(AchievementUpdateModel achievement)
    {
        var achievementInDb = await _database.Achievements.FirstOrDefaultAsync(a => a.Id == achievement.Id);
        if (achievementInDb is null) 
            throw new Exception("Achievement not found.");

        _database.Achievements.Entry(achievementInDb).CurrentValues.SetValues(achievement);
        await _database.SaveChangesAsync();
        
        return achievementInDb;
    }
}