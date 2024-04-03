using BudgetBuddy.Contracts.ModelRequest.CreateModels;

namespace BudgetBuddy.Services.TransactionServices;

public interface ITransactionService
{
    Task HandleAccountBalance(TransactionCreateRequest transaction);
}