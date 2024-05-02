using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.TransactionEnums;
using BudgetBuddy.Services.Repositories.Account;

namespace BudgetBuddy.Services.TransactionServices;

public class TransactionService : ITransactionService
{
    private readonly IAccountRepository _accountRepository;
    
    public TransactionService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task HandleAccountBalance(TransactionCreateRequest transaction)
    {
        var account = await _accountRepository.GetById(transaction.AccountId);
        if (transaction.Type == TransactionType.Expense && account.Balance - transaction.Amount < 0)
        {
            throw new InvalidDataException("Insufficient funds to complete the transaction.");
        }

        decimal updatedBalance;
        if (transaction.Type == TransactionType.Expense)
        {
            updatedBalance = account.Balance - transaction.Amount;
        }
        else
        {
            updatedBalance = account.Balance + transaction.Amount;
        }
        AccountUpdateRequest updateRequest = new AccountUpdateRequest(account.Id, updatedBalance,
            account.Name, account.Type, account.UserId);
        await _accountRepository.UpdateAccount(updateRequest);
    }
}