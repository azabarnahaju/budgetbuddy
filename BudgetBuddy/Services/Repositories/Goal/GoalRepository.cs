namespace BudgetBuddy.Services.Repositories.Goal;

using Contracts.ModelRequest.CreateModels;
using Data;
using Model;
using Microsoft.EntityFrameworkCore;

public class GoalRepository : IGoalRepository
{
    private readonly BudgetBuddyContext _database;

    public GoalRepository(BudgetBuddyContext database)
    {
        _database = database;
    }
    
    public async Task<Goal[]> GetAllGoalsByAccountId(int accountId, bool? completed)
    {
        try
        {
            var result = _database.Goals
                .Where(goal => goal.AccountId == accountId)
                .ToArray();
            if (result == null)
            {
                throw new KeyNotFoundException("Goals not found.");
            }

            return result;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("An unexpected error occured, cannot get goals");
        }
    }
    
    public async Task<Goal> CreateGoal(GoalCreateRequest goal)
    {
        try
        {
            var goalToCreate = new Goal()
            {
                AccountId = goal.AccountId,
                UserId = goal.UserId,
                Completed = goal.Completed,
                CurrentProgress = goal.CurrentProgress,
                StartDate = goal.StartDate,
                Type = goal.Type,
                Target = goal.Target
            };
            var newGoal = await _database.Goals.AddAsync(goalToCreate);
            await _database.SaveChangesAsync();
            return newGoal.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Cannot create new goal.");
        }
    }
    
    public async Task<Goal> UpdateGoal(Goal goal)
    {
        try
        {
            var existingGoal = await _database.Goals.FirstOrDefaultAsync(c => c.Id == goal.Id);

            if (existingGoal == null)
            {
                throw new KeyNotFoundException("Failed to update. Goal not found.");
            }
            
            _database.Entry(existingGoal).CurrentValues.SetValues(goal);
            await _database.SaveChangesAsync();

            return existingGoal;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("An unexpected error occured. Goal not updated.");
        }
    }
    
    public async Task DeleteGoal(int id)
    {
        try
        {
            var goalToDelete = await _database.Goals.FirstOrDefaultAsync(c => c.Id == id);

            if (goalToDelete == null)
            {
                throw new KeyNotFoundException("Failed to delete. Account not found.");
            }
            
            _database.Goals.Remove(goalToDelete);
            await _database.SaveChangesAsync();
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Goal not deleted. An unexpected error occured.");
        }
    }
}