using BudgetBuddy.Data;
using BudgetBuddy.Model.CreateModels;
using BudgetBuddy.Model.UpdateModels;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services.Repositories.Account;

using System.Data;
using Model;

public class AccountRepository : IAccountRepository
{
    private readonly BudgetBuddyContext _budgetBuddyContext;
    public AccountRepository(BudgetBuddyContext budgetBuddyContext)
    {
        _budgetBuddyContext = budgetBuddyContext;
    }

    public async Task<List<Account>> GetAll()
    {
        try
        {
            return await _budgetBuddyContext.Accounts.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Cannot get accounts.");
        }
    }

    public async Task<Account> GetById(int id)
    {
        try
        {
            var result = await _budgetBuddyContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == id);
            if (result == null)
            {
                throw new KeyNotFoundException("Account not found.");
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
            throw new Exception("An unexpected error occured, cannot get account");
        }
    }

    public async Task<Account> CreateAccount(AccountInputModel account)
    {
        try
        {
            var accoutToCreate = new Account()
            {
                Balance = account.Balance,
                Date = account.Date,
                Name = account.Name,
                UserId = account.UserId,
                Type = account.Type
            };
            var newAccount = await _budgetBuddyContext.Accounts.AddAsync(accoutToCreate);
            await _budgetBuddyContext.SaveChangesAsync();
            return newAccount.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Cannot create new account.");
        }
    }

    public async Task<Account> UpdateAccount(AccountUpdateModel account)
    {
        try
        {
            var existingAccount = await _budgetBuddyContext.Accounts.FirstOrDefaultAsync(c => c.Id == account.Id);

            if (existingAccount == null)
            {
                throw new KeyNotFoundException("Failed to update. Account not found.");
            }
            
            _budgetBuddyContext.Entry(existingAccount).CurrentValues.SetValues(account);
            await _budgetBuddyContext.SaveChangesAsync();

            return existingAccount;
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("An unexpected error occured. Account not updated.");
        }
    }

    public async Task DeleteAccount(int id)
    {
        try
        {
            var accountToDelete = await _budgetBuddyContext.Accounts.FirstOrDefaultAsync(c => c.Id == id);

            if (accountToDelete == null)
            {
                throw new KeyNotFoundException("Failed to delete. Account not found.");
            }
            
            _budgetBuddyContext.Accounts.Remove(accountToDelete);
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
            throw new Exception("Account not deleted. An unexpected error occured.");
        }
    }
}