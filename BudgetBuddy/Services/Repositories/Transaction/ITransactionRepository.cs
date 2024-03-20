using BudgetBuddy.Model.CreateModels;
using BudgetBuddy.Model.UpdateModels;

namespace BudgetBuddy.Services.Repositories.Transaction;

using Model.Enums;
using Model;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactions();
    Task<Transaction> GetTransaction(int id);
    Task<Transaction> AddTransaction(TransactionInputModel transaction);
    Task<Transaction> UpdateTransaction(TransactionUpdateModel transaction);
    void DeleteTransaction(int id);

    Task<IEnumerable<Transaction>> FilterTransactions(TransactionType transactionType);
    Task<IEnumerable<Transaction>> FinancialTransactions(TransactionCategoryTag tag);
}