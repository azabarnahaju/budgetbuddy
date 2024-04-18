namespace BudgetBuddy.Services.FinancialNewsService;

public interface IFinancialNewsProvider
{
    Task<string> GetFinancialNews();
}