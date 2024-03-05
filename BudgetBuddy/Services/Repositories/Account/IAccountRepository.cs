namespace BudgetBuddy.Services.Repositories.Account;

using Model;

public interface IAccountRepository
{
    Task<List<Account>> GetAll();
    Task<Account> GetById(int id);
    Task<Account> CreateAccount(Account account);
    Task<Account> UpdateAccount(Account account);
    Task DeleteAccount(int id);
}