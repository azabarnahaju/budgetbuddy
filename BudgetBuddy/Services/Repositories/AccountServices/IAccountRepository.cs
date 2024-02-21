using BudgetBuddy.Model.AccountModels;

namespace BudgetBuddy.Services.Repositories.AccountServices;

public interface IAccountRepository
{
    List<Account> GetAll();
    Account GetById(int id);
    Account CreateAccount(Account account);
    Account UpdateAccount(Account account);
    void DeleteAccount(int id);
}