using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;

namespace BudgetBuddy.Services.Repositories.Transaction;

using Model.Enums;
using Model;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactions();
    Task<Transaction> GetTransaction(int id);
    Task<Transaction> AddTransaction(TransactionCreateRequest transaction);
    Task<Transaction> UpdateTransaction(TransactionUpdateRequest transaction);
    Task DeleteTransaction(int id);

    Task<IEnumerable<Transaction>> FilterTransactions(TransactionType transactionType);
    Task<IEnumerable<Transaction>> FinancialTransactions(TransactionCategoryTag tag);
    Task<IEnumerable<Transaction>> GetExpenseTransactions(int accountId, DateTime start, DateTime end);
}