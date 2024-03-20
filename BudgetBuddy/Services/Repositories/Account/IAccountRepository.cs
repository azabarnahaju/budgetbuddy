using BudgetBuddy.Model.CreateModels;
using BudgetBuddy.Model.UpdateModels;

namespace BudgetBuddy.Services.Repositories.Account;

using Model;

public interface IAccountRepository
{
    Task<List<Account>> GetAll();
    Task<Account> GetById(int id);
    Task<Account> CreateAccount(AccountInputModel account);
    Task<Account> UpdateAccount(AccountUpdateModel account);
    Task DeleteAccount(int id);
}