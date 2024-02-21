namespace BudgetBuddy.Services.Repositories.Account;

using Model;

public interface IAccountRepository
{
    List<Account> GetAll();
    Account GetById(int id);
    Account CreateAccount(Account account);
    Account UpdateAccount(Account account);
    void DeleteAccount(int id);
}