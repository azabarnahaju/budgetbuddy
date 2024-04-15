
﻿using BudgetBuddy.Data;
﻿using BudgetBuddy.Contracts.ModelRequest;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;
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

    public async Task<IEnumerable<Achievement>> GetAllAchievementsByUserId(string userId)
    {
        return await _database.Users.Where(a => a.Id == userId)
            .SelectMany(a => a.Achievements)
            .ToListAsync();
    }

    public async Task<IEnumerable<Achievement>> AddAchievement(IEnumerable<AchievementCreateRequest> achievements)
    {
        var createdAchievements = new List<Achievement>();
        foreach (var achievement in achievements)
        {
            var isExists = await _database.Achievements.FirstOrDefaultAsync(a => a.Name == achievement.Name);
            if (isExists != null)
            {
                throw new Exception("You're trying to add duplicate achievements.");
            }
            var achievementToCreate = new Achievement
            {
                Description = achievement.Description,
                Name = achievement.Name
            };
            var result = await _database.Achievements.AddAsync(achievementToCreate);
            createdAchievements.Add(result.Entity);
        }
        
        await _database.SaveChangesAsync();
        
        return createdAchievements;
    }

    public async Task DeleteAchievement(int id)
    {
        if (!await _database.Achievements.AnyAsync() || await _database.Achievements.AllAsync(a => a.Id != id)) 
            throw new Exception("Achievement is not found.");
        
        _database.Achievements.Remove(await GetAchievement(id));
        await _database.SaveChangesAsync();
    }
    

    public async Task<Achievement> UpdateAchievement(AchievementUpdateRequest achievement)
    {
        var achievementInDb = await _database.Achievements.FirstOrDefaultAsync(a => a.Id == achievement.Id);
        if (achievement is null)
            throw new Exception("Achievement not found.");

        _database.Achievements.Entry(achievementInDb).CurrentValues.SetValues(achievement);
        await _database.SaveChangesAsync();
        
        return achievementInDb;
    }
}