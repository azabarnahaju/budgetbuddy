namespace BudgetBuddy.Services.Repositories.Transaction;

using Model.Enums;
using Model;

public interface ITransactionRepository
{
    IList<Transaction> _transactions { get; set; }
    IEnumerable<Transaction> GetAllTransactions();
    Transaction GetTransaction(int id);
    void AddTransaction(Transaction transaction);
    Transaction UpdateTransaction(Transaction transaction);
    void DeleteTransaction(int id);

    IEnumerable<Transaction> FilterTransactions(TransactionType transactionType);
    IEnumerable<Transaction> FinancialTransactions(TransactionCategoryTag tag);
}