using BudgetBuddy.Data;
using BudgetBuddy.Model;

namespace BudgetBuddy.Services.AchievementService;

public class AchievementSeeder
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
                new Achievement
                    { Name = "Pioneer", Description = "You've recorded your first expense transaction!" },
                new Achievement
                    { Name = "Big Spender", Description = "You've recorded 5 expense transactions!" },
                new Achievement
                    { Name = "Money Bags", Description = "You've recorded 10 expense transactions!" },

                new Achievement
                    { Name = "Money Maker", Description = "You've recorded your first income transaction!" },
                new Achievement
                    { Name = "Wealth Builder", Description = "You've recorded 5 income transactions!" },
                new Achievement
                    { Name = "Financial Wizard", Description = "You've recorded 10 income transactions!" },

                new Achievement { Name = "Account Holder", Description = "You've created your first account!" },

                new Achievement
                    { Name = "Penny Pincher", Description = "You've saved up $500 in your account!" },
                new Achievement { Name = "Frugal", Description = "You've saved up $1000 in your account!" },
                new Achievement { Name = "Thrifty", Description = "You've saved up $1500 in your account!" },

                new Achievement { Name = "Goal Setter", Description = "You've set your first goal!" },
                new Achievement { Name = "Goal Achiever", Description = "You've set 3 goals!" },
                new Achievement { Name = "Master of Goals", Description = "You've set 5 goals!" },
                
                new Achievement { Name = "Goal Getter", Description = "You've completed your first goal!" },
                new Achievement { Name = "Goal Digger", Description = "You've completed 3 goals!" },
                new Achievement { Name = "Goal Crusher", Description = "You've completed 5 goals!" },

                new Achievement
                    { Name = "Five-Star Dabbler", Description = "You've used 5 different categories!" },
                new Achievement { Name = "Jack of All Trades", Description = "You've used 10 categories!" },
                new Achievement { Name = "Master of All", Description = "You've used ALL categories!" }
            };
            
            await _dbContext.Achievements.AddRangeAsync(achievements);
            await _dbContext.SaveChangesAsync();
        }
    }
}