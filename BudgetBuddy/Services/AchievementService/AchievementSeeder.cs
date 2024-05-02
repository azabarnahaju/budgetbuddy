using BudgetBuddy.Data;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.AchievementEnums;
using BudgetBuddy.Model.Enums.TransactionEnums;

namespace BudgetBuddy.Services.AchievementService;

public class AchievementSeeder : IAchievementSeeder
{
    private readonly BudgetBuddyContext _dbContext;
    
    public AchievementSeeder(BudgetBuddyContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAchievementsAsync()
    {
        if (!_dbContext.Achievements.Any())
        {
            var achievements = new List<Achievement>
            {
                new Achievement("Pioneer", AchievementType.Exploration, 1, AchievementObjectiveType.Transaction),
                new Achievement("Money Tracker", AchievementType.Exploration, 10, AchievementObjectiveType.Transaction),
                new Achievement("Transaction Pro", AchievementType.Exploration, 25, AchievementObjectiveType.Transaction),
                new Achievement("Account Starter", AchievementType.Exploration, 1, AchievementObjectiveType.Account),
                new Achievement("Multi-Account Holder", AchievementType.Exploration, 3, AchievementObjectiveType.Account),
                new Achievement("Master of Accounts", AchievementType.Exploration, 5, AchievementObjectiveType.Account),
                new Achievement("Income Beginner", AchievementType.Exploration, 1, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Income Earner", AchievementType.Exploration, 10, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Income Guru", AchievementType.Exploration, 25, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Master of Income", AchievementType.Exploration, 50, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Expense Beginner", AchievementType.Exploration, 1, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Expense Proficient", AchievementType.Exploration, 10, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Big Spender", AchievementType.Exploration, 25, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Master of Expenses", AchievementType.Exploration, 50, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement ("Goal Getter", AchievementType.Exploration, 1, AchievementObjectiveType.Goal),
                new Achievement ("Goal Digger", AchievementType.Exploration, 3, AchievementObjectiveType.Goal),
                new Achievement ("Goal Crusher", AchievementType.Exploration, 5, AchievementObjectiveType.Goal),
                new Achievement("Penny Pincher", AchievementType.AmountBased, 100, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Budget Boss", AchievementType.AmountBased, 500, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Financial Guru", AchievementType.AmountBased, 2000, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Money Master", AchievementType.AmountBased, 5000, AchievementObjectiveType.TransactionType, TransactionType.Expense),
                new Achievement("Money Maker", AchievementType.AmountBased, 100, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Cash Flow Captain", AchievementType.AmountBased, 500, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Income Innovator", AchievementType.AmountBased, 2000, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Wealth Wizard", AchievementType.AmountBased, 5000, AchievementObjectiveType.TransactionType, TransactionType.Income),
                new Achievement("Entertainment Explorer", AchievementType.AmountBased, 50, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment),
                new Achievement("Leisure Luminary", AchievementType.AmountBased, 150, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment),
                new Achievement("Fun Fund Fancier", AchievementType.AmountBased, 500, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment),
                new Achievement("Entertainment Enthusiast", AchievementType.AmountBased, 1000, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment),
            };
            
            await _dbContext.Achievements.AddRangeAsync(achievements);
            await _dbContext.SaveChangesAsync();
        }
    }
}