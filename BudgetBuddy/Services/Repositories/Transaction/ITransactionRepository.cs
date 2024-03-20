namespace BudgetBuddy.Services.Repositories.Transaction;

using Model.Enums;
using Model;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllTransactions();
    Task<Transaction> GetTransaction(int id);
    void AddTransaction(Transaction transaction);
    Task<Transaction> UpdateTransaction(Transaction transaction);
    void DeleteTransaction(int id);

    Task<IEnumerable<Transaction>> FilterTransactions(TransactionType transactionType);
    Task<IEnumerable<Transaction>> FinancialTransactions(TransactionCategoryTag tag);
    Task<IEnumerable<Transaction>> GetExpenseTransactions(int accountId, DateTime start, DateTime end);
}