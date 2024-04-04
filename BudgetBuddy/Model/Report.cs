using System.ComponentModel.DataAnnotations.Schema;
using BudgetBuddy.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Model;

public class Report
{
    public int Id { get; set; }
    public Account Account { get; set; } = null!;
    public int AccountId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ReportType ReportType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Transaction> Transactions { get; set; }
    public HashSet<TransactionCategoryTag> Categories { get; set; }
    public Dictionary<TransactionCategoryTag, decimal> SpendingByTags { get; set; }
    [Precision(14, 2)]
    public decimal AverageSpendingDaily { get; set; }
    [Precision(14, 2)]
    public decimal AverageSpendingTransaction { get; set; }
    public TransactionCategoryTag MostSpendingTag { get; set; }
    public DateTime MostSpendingDay { get; set; }
    [Precision(14, 2)]
    public decimal SumExpense { get; set; }
    [Precision(14, 2)]
    public decimal SumIncome { get; set; }
    [Precision(14, 2)]
    public decimal BiggestExpense { get; set; }
   
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        Report other = (Report)obj;

        return Id == other.Id &&
               AccountId == other.AccountId &&
               CreatedAt == other.CreatedAt &&
               ReportType == other.ReportType &&
               StartDate == other.StartDate &&
               EndDate == other.EndDate &&
               AverageSpendingDaily == other.AverageSpendingDaily &&
               AverageSpendingTransaction == other.AverageSpendingTransaction &&
               MostSpendingTag == other.MostSpendingTag &&
               MostSpendingDay == other.MostSpendingDay &&
               SumExpense == other.SumExpense &&
               SumIncome == other.SumIncome &&
               BiggestExpense == other.BiggestExpense;
    }
}