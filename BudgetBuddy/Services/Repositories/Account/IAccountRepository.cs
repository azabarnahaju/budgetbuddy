using BudgetBuddy.Contracts.ModelRequest;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;

namespace BudgetBuddy.Services.Repositories.Account;

using Model;

public interface IAccountRepository
{
    Task<List<Account>> GetAll();
    Task<Account> GetById(int id);
    Task<Account> CreateAccount(AccountCreateRequest account);
    Task<Account> UpdateAccount(AccountUpdateRequest account);
    Task DeleteAccount(int id);
}