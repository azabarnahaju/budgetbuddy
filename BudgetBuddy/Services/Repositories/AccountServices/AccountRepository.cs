using System.Data;
using BudgetBuddy.Model.AccountModels;

namespace BudgetBuddy.Services.Repositories.AccountServices;

public class AccountRepository : IAccountRepository
{
    private readonly List<Account> _accounts = new ();

    public List<Account> GetAll()
    {
        try
        {
            return _accounts;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Cannot get accounts.");
        }
    }

    public Account GetById(int id)
    {
        try
        {
            Account? result = _accounts.Find(acc => acc.Id == id);
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

    public Account CreateAccount(Account account)
    {
        try
        {
            if (_accounts.Any(acc => acc.Id == account.Id))
                throw new InvalidConstraintException("Account already exists.");
            _accounts.Add(account);
            return account;
        }
        catch (InvalidConstraintException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Cannot create new account.");
        }
    }

    public Account UpdateAccount(Account account)
    {
        try
        {
            int accountId = _accounts.FindIndex(acc => acc.Id == account.Id);
            if (accountId < 0)
            {
                throw new KeyNotFoundException("Failed to update. Account not found.");
            }

            _accounts[accountId] = account;
            return account;
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

    public void DeleteAccount(int id)
    {
        try
        {
            Account? account = _accounts.Find(acc => acc.Id == id);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }

            _accounts.Remove(account);
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