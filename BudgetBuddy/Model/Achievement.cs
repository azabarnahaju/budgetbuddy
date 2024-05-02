using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.AchievementEnums;
using BudgetBuddy.Model.Enums.TransactionEnums;

namespace BudgetBuddy.Model;

public class Achievement
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public AchievementType Type { get; init; }
    public int Criteria { get; init; }
    public AchievementObjectiveType Objective { get; init; }
    private TransactionType? _transactionType;
    private TransactionCategoryTag? _transactionCategoryTag;

    public TransactionType? TransactionType
    {
        get => _transactionType;
        set
        {
            _transactionType = value;
            if (Objective == AchievementObjectiveType.TransactionType && _transactionType is null)
            {
                throw new InvalidOperationException("TransactionType cannot be null when Objective is TransactionType");
            }
        }
    }

    public TransactionCategoryTag? TransactionTag
    {
        get => _transactionCategoryTag;
        set
        {
            _transactionCategoryTag = value;
            if (Objective == AchievementObjectiveType.TransactionTag && _transactionCategoryTag is null)
            {
                throw new InvalidOperationException("TransactionTag cannot be null when Objective is TransactionTag");
            }
        }
    }
    
    public HashSet<ApplicationUser> Users { get; init; }

    public Achievement(string name, AchievementType type, int criteria, AchievementObjectiveType objective, TransactionType? transactionType = null, TransactionCategoryTag? transactionTag = null)
    {
        Name = name;
        Type = type;
        Criteria = criteria;
        Objective = objective;
        TransactionType = transactionType;
        TransactionTag = transactionTag;
        Description = GetDescription();
        Users = new HashSet<ApplicationUser>();
    }

    private string GetDescription()
    {
        var description = "";
        switch (Type)
        {
            case AchievementType.Exploration:
            {
                description = Objective == AchievementObjectiveType.Goal ? $"You have set {Criteria} {Objective}" : $"You have created {Criteria} {Objective}";
                
                if (Criteria > 1)
                {
                    description += "s!";
                }
                else
                {
                    description += "!";
                }
                
                break;
            }
            case AchievementType.AmountBased:
                description = $"You have reached ${Criteria} in your {Objective}(s)!";
                break;
        } 
        
        return description;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Achievement || obj is null) return false;

        var other = (Achievement)obj;

        return other.Id == this.Id && other.Name == this.Name && other.Description == this.Description;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Description, Users);
    }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Description: {Description}, Users: {Users}";
    }
}