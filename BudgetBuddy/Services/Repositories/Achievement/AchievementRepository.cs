using BudgetBuddy.Data;
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

    public async Task<IEnumerable<Achievement>> AddAchievement(IEnumerable<Achievement> achievements)
    {
        foreach (var achievement in achievements)
        {
            if (await _database.Achievements.AnyAsync(a => a.Id == achievement.Id)) 
                throw new Exception($"Achievement with ID {achievement.Id} already exists.");
        }
        
        if (achievements.Select(achievement => achievement.Id).Distinct().Count() != achievements.Count())
            throw new Exception("You're trying to add duplicate achievements.");
        
        _database.Achievements.AddRange(achievements);
        await _database.SaveChangesAsync();
        
        return achievements;
    }

    public async Task DeleteAchievement(int id)
    {
        if (!await _database.Achievements.AnyAsync() || await _database.Achievements.AllAsync(a => a.Id != id)) 
            throw new Exception("Achievement is not found.");
        
        _database.Achievements.Remove(await GetAchievement(id));
        await _database.SaveChangesAsync();
    }
    
    public async Task<Achievement> UpdateAchievement(Achievement achievement)
    {
        var achievementInDb = await _database.Achievements.FirstOrDefaultAsync(a => a.Id == achievement.Id);
        if (achievementInDb is null) 
            throw new Exception("Achievement not found.");

        _database.Achievements.Entry(achievementInDb).CurrentValues.SetValues(achievement);
        await _database.SaveChangesAsync();
        
        return await _database.Achievements.FirstAsync(a => a.Id == achievement.Id);
    }
}