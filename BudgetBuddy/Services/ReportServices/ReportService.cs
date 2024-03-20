using System.Runtime.InteropServices.JavaScript;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Services.Repositories.Transaction;

namespace BudgetBuddy.Services.ReportServices;

public class ReportService : IReportService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<ReportService> _logger;

    public ReportService(ILogger<ReportService> logger, ITransactionRepository transactionRepository)
    {
        _logger = logger;
        _transactionRepository = transactionRepository;
    }
    
    public async Task<Report> CreateReport(Account account, ReportType type, DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            var (start, end) = GetDates(type, startDate, endDate);
            var transactionsPeriod = await _transactionRepository.GetExpenseTransactions(account.Id, start, end);
            var tags = await GetCategories(transactionsPeriod);
            var spendingByTags = GetSpendingByCategories(tags, transactionsPeriod);
            
            return new Report
            {
                ReportType = type,
                CreatedAt = DateTime.Now,
                Account = account,
                AccountId = account.Id,
                StartDate = start,
                EndDate = end,
                Transactions = transactionsPeriod.ToList(),
                Categories = tags,
                SpendingByTags = spendingByTags,
                AverageSpendingDaily = GetAvgSpendingDaily(transactionsPeriod, start, end),
                AverageSpendingTransaction = transactionsPeriod.Average(t => t.Amount),
                MostSpendingTag = GetMostSpendingTag(spendingByTags),
                MostSpendingDay = GetMostSpendingDay(transactionsPeriod),
                SumExpense = transactionsPeriod.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount),
                SumIncome = transactionsPeriod.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                BiggestExpense = transactionsPeriod.Max(t => t.Amount)
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private (DateTime, DateTime) GetDates(ReportType type, DateTime? startDate, DateTime? endDate)
    {
        switch (type)
        {
            case ReportType.Monthly:
                return (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today);
            case ReportType.Weekly:
                return (GetStartOfCurrentWeek(DayOfWeek.Monday), DateTime.Today);
            case ReportType.Yearly:
                return (new DateTime(DateTime.Today.Year, 1, 1), DateTime.Today);
            case ReportType.Last7Days:
                return (DateTime.Today.AddDays(-7), DateTime.Today);
            case ReportType.Last30Days:
                return (DateTime.Today.AddDays(-30), DateTime.Today);
            case ReportType.Custom:
                if (startDate is null || endDate is null)
                    throw new Exception("Missing date(s).");
                return ((DateTime)startDate, (DateTime)endDate);
            default:
                throw new Exception("Invalid report type.");
        }
    }
    
    private DateTime GetStartOfCurrentWeek(DayOfWeek startOfWeek)
    {
        int diff = (7 + (DateTime.Today.DayOfWeek - startOfWeek)) % 7;
        return DateTime.Today.AddDays(-1 * diff).Date;
    }

    private async Task<HashSet<TransactionCategoryTag>> GetCategories(IEnumerable<Transaction> transactions)
    {
        try
        {
            return transactions.Select(t => t.Tag).ToHashSet();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error retrieving categories.");
        }
    }

    private Dictionary<TransactionCategoryTag, decimal> GetSpendingByCategories(HashSet<TransactionCategoryTag> tags, IEnumerable<Transaction> transactions)
    {
        var result = new Dictionary<TransactionCategoryTag, decimal>();
        foreach (var tag in tags)
        {
            var sumOfExpense = transactions.Where(t => t.Tag == tag && t.Type == TransactionType.Expense).Sum(t => t.Amount);
            result[tag] = sumOfExpense;
        }

        return result;
    }

    private decimal GetAvgSpendingDaily(IEnumerable<Transaction> transactions, DateTime start, DateTime end)
    {
        var days = (end - start).Days;
        return transactions.Sum(t => t.Amount) / days;
    }

    private TransactionCategoryTag GetMostSpendingTag(Dictionary<TransactionCategoryTag, decimal> spendingByTags)
    {
        return spendingByTags.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).First();
    }

    private DateTime GetMostSpendingDay(IEnumerable<Transaction> transactions)
    {
        return transactions.GroupBy(x => x.Date).Select(x => new { Date = x.Key, Amount = x.Sum(t => t.Amount) }).OrderByDescending(x => x.Amount).First().Date;
    }
}