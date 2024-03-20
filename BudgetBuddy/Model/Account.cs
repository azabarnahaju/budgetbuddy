using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Model;

public class Account
{
    public int Id { get; init; }
    public DateTime Date { get; init; } 
    [Precision(14, 2)]
    public decimal Balance { get; init; } 
    public string Name { get; init; } 
    public string Type { get; init; } 
    public ApplicationUser User { get; init; } 
    public string UserId { get; init; }
    public List<Transaction> Transactions { get; init; } 
    public List<Report> Reports { get; init; }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        Account other = (Account)obj;

        return Id == other.Id &&
               Date == other.Date &&
               Balance == other.Balance &&
               Name == other.Name &&
               Type == other.Type &&
               UserId == other.UserId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Date, Balance, Name, Type, UserId);
    }
}