using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;
using BudgetBuddy.Model.Enums.TransactionEnums;

namespace BudgetBuddy.Services.Repositories.Transaction;

using Model.Enums;
using Model;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactions();
    Task<Transaction> GetTransaction(int id);

    Task<IEnumerable<Transaction>> GetTransactionsByAccount(int accountId, DateTime? startDate = null,
        DateTime? endDate = null);
    Task<Transaction> AddTransaction(TransactionCreateRequest transaction);
    Task<Transaction> UpdateTransaction(TransactionUpdateRequest transaction);
    Task DeleteTransaction(int id);
    Task<IEnumerable<Transaction>> FilterTransactions(TransactionType transactionType);
    Task<IEnumerable<Transaction>> FinancialTransactions(TransactionCategoryTag tag);
    Task<IEnumerable<Transaction>> GetExpenseTransactions(int accountId, DateTime start, DateTime end);
}