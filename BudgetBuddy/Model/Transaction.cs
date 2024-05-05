using BudgetBuddy.Model.Enums.TransactionEnums;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Model;

using Enums;

public class Transaction
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public string Name { get; init; }
    [Precision(14, 2)]
    public decimal Amount { get; init; }
    public TransactionCategoryTag Tag { get; init; }
    public TransactionType Type { get; init; }
    public Account Account { get; init; } = null!;
    public int AccountId { get; init; }
    public List<Report> Reports { get; init; }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        Transaction other = (Transaction)obj;

        return Id == other.Id &&
               Date == other.Date &&
               AccountId == other.AccountId &&
               Type == other.Type &&
               Name == other.Name &&
               Tag == other.Tag &&
               Type == other.Type;
    }
};

