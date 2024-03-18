using BudgetBuddy.Data;
using BudgetBuddy.Model;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services.Repositories.Goal;

public class GoalRepository : IGoalRepository
{
    private readonly BudgetBuddyContext _database;

    public GoalRepository(BudgetBuddyContext database)
    {
        _database = database;
    }
    
    public async Task<GoalModel[]> GetAllGoalsByUserId(int userId)
    {
        try
        {
            var result = _database.GoalModel.Where(goal => goal.UserId == userId).ToArray();
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
    
    public async Task<GoalModel> CreateGoal(GoalModel goal)
    {
        try
        {
            var newGoal = await _database.GoalModel.AddAsync(goal);
            await _database.SaveChangesAsync();
            return newGoal.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Cannot create new goal.");
        }
    }
    
    public async Task<GoalModel> UpdateGoal(GoalModel goal)
    {
        try
        {
            var existingGoal = await _database.GoalModel.FirstOrDefaultAsync(c => c.Id == goal.Id);

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
            var goalToDelete = await _database.GoalModel.FirstOrDefaultAsync(c => c.Id == id);

            if (goalToDelete == null)
            {
                throw new KeyNotFoundException("Failed to delete. Account not found.");
            }
            
            _database.GoalModel.Remove(goalToDelete);
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