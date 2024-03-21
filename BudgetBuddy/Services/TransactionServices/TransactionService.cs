using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Services.Repositories.Account;
using BudgetBuddy.Services.Repositories.Transaction;

namespace BudgetBuddy.Services.TransactionServices;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    
    public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task HandleAccountBalance(TransactionCreateRequest transaction)
    {
        var account = await _accountRepository.GetById(transaction.AccountId);
        if (account.Balance - transaction.Amount < 0)
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