using BudgetBuddy.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services.Repositories.User;

using Model;

public class UserRepository : IUserRepository
{
    private readonly BudgetBuddyContext _budgetBuddyContext;

    public UserRepository(BudgetBuddyContext budgetBuddyContext)
    {
        _budgetBuddyContext = budgetBuddyContext;
    }
    
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        try
        {
            return await _budgetBuddyContext.Users.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Cannot get users.");
        }
    }

    public async Task<User> GetUser(string email)
    {
        try
        {
            var result = await _budgetBuddyContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (result == null)
            {
                throw new KeyNotFoundException("User not found.");
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
            throw new Exception("An unexpected error occured, cannot get user");
        }
    }

    public async Task<User> GetUser(int id)
    {
        try
        {
            var result = await _budgetBuddyContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (result == null)
            {
                throw new KeyNotFoundException("User not found.");
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
            throw new Exception("An unexpected error occured, cannot get user");
        }
    }

    public async Task<User> AddUser(User user)
    {
        try
        {
            var newUser = await _budgetBuddyContext.Users.AddAsync(user);
            await _budgetBuddyContext.SaveChangesAsync();
            return newUser.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Cannot create new user.");
        }
    }

    public async Task<User> UpdateUser(User user)
    {
        try
        {
            var existingUser = await _budgetBuddyContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException("Failed to update. User not found.");
            }
            
            _budgetBuddyContext.Entry(existingUser).CurrentValues.SetValues(user);
            await _budgetBuddyContext.SaveChangesAsync();

            return existingUser;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("An unexpected error occured. User not updated.");
        }
    }

    public async Task DeleteUser(int id)
    {
        try
        {
            var userToDelete = await _budgetBuddyContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userToDelete == null)
            {
                throw new KeyNotFoundException("Failed to delete. User not found.");
            }
            
            _budgetBuddyContext.Users.Remove(userToDelete);
            await _budgetBuddyContext.SaveChangesAsync();
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User not deleted. An unexpected error occured.");
        }
    }
}