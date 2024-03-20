using BudgetBuddy.Data;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;

namespace BudgetBuddy.Services.AchievementService;

public class AchievementService : IAchievementService
{
    
    private readonly BudgetBuddyContext _context;
    
    public AchievementService(BudgetBuddyContext context)
    {
        _context = context;
    }
    
    public async Task UpdateAchievements(ApplicationUser user)
    {
        await BudgetMasterAchievement(user, 1000);
        await TransactionTrackerAchievement(user);
        await AccountAchievement(user);
        await SavingsAchievement(user);
        //await FirstGoalAchievement(user);
        //await GoalVisionary(user);
        await CategorizeAchievement(user);
    }

    public async Task BudgetMasterAchievement(ApplicationUser user, decimal budgetAmount)
    {
        
        var transactions= user.Accounts.SelectMany(account => account.Transactions).Where(transaction => transaction.Date >= DateTime.Now.AddDays(-30)).ToList();
        var totalSpent = transactions.Where(transaction => transaction.Type == TransactionType.Expense).Sum(transaction => transaction.Amount);
        
        if (totalSpent < budgetAmount)
        {
            var budgetMaster = new Achievement
            {
                Name = "Budget Master",
                Description = "You've spent less than your budget in the past 30 days!"
            };
            if (user.Achievements.Any(a => a.Name != budgetMaster.Name)) user.Achievements.Add(budgetMaster);
            if (_context.Achievements.Any(a => a.Id != budgetMaster.Id)) _context.Achievements.Add(budgetMaster);
            
            await _context.SaveChangesAsync();
            
        }
    }

    public async Task TransactionTrackerAchievement(ApplicationUser user)
    {
        var totalTransactions = user.Accounts.Sum(account => account.Transactions.Where(transaction =>  transaction.Type == TransactionType.Expense).ToList().Count);

        switch (totalTransactions)
        {
            case 1:
                var pioneer = new Achievement
                {
                    Name = "Pioneer",
                    Description = "You've recorded your first transaction!"
                };
                if (user.Achievements.Any(a => a.Name != pioneer.Name)) user.Achievements.Add(pioneer);
                if (_context.Achievements.Any(a => a.Id != pioneer.Id)) _context.Achievements.Add(pioneer);
                await _context.SaveChangesAsync();
                break;
            case 5:
                var bigSpender = new Achievement
                {
                    Name = "Big Spender",
                    Description = "You've recorded 5 transactions!"
                };
                if (user.Achievements.Any(a => a.Name != bigSpender.Name)) user.Achievements.Add(bigSpender);
                if (_context.Achievements.Any(a => a.Id != bigSpender.Id)) _context.Achievements.Add(bigSpender);
                await _context.SaveChangesAsync();
                break;
            case 10:
                var moneyBags = new Achievement
                {
                    Name = "Money Bags",
                    Description = "You've recorded 10 transactions!"
                };
                if (user.Achievements.Any(a => a.Name != moneyBags.Name)) user.Achievements.Add(moneyBags);
                if (_context.Achievements.Any(a => a.Id != moneyBags.Id)) _context.Achievements.Add(moneyBags);
                await _context.SaveChangesAsync();
                break;
        }
    }

    public async Task AccountAchievement(ApplicationUser user)
    {
        if (user.Accounts.Count > 0)
        {
            var accountHolder = new Achievement
            {
                Name = "Account Holder",
                Description = "You've created your first account!"
            };
            if (user.Achievements.Any(a => a.Name != accountHolder.Name)) user.Achievements.Add(accountHolder);
            if (_context.Achievements.Any(a => a.Id != accountHolder.Id)) _context.Achievements.Add(accountHolder);
            await _context.SaveChangesAsync();
            
        }
    }

    public async Task SavingsAchievement(ApplicationUser user)
    {
        var totalSavings = user.Accounts.Sum(account => account.Transactions.Where(transaction => transaction.Type == TransactionType.Income).Sum(transaction => transaction.Amount));

        switch (totalSavings)
        {
            case 500:
                var pennyPincher = new Achievement
                {
                    Name = "Penny Pincher",
                    Description = "You've saved $500!"
                };
                if (user.Achievements.Any(a => a.Name != pennyPincher.Name)) user.Achievements.Add(pennyPincher);
                if (_context.Achievements.Any(a => a.Id != pennyPincher.Id)) _context.Achievements.Add(pennyPincher);
                await _context.SaveChangesAsync();
                break;
            case 1000:
                var frugal = new Achievement
                {
                    Name = "Frugal",
                    Description = "You've saved $1000!"
                };
                if (user.Achievements.Any(a => a.Name != frugal.Name)) user.Achievements.Add(frugal);
                if (_context.Achievements.Any(a => a.Id != frugal.Id)) _context.Achievements.Add(frugal);
                await _context.SaveChangesAsync();
                break;
            case 1500:
                var thrifty = new Achievement
                {
                    Name = "Thrifty",
                    Description = "You've saved $1500!"
                };
                if (user.Achievements.Any(a => a.Name != thrifty.Name)) user.Achievements.Add(thrifty);
                if (_context.Achievements.Any(a => a.Id != thrifty.Id)) _context.Achievements.Add(thrifty);
                await _context.SaveChangesAsync();
                break;
        }
    }

    // public async Task FirstGoalAchievement(ApplicationUser user)
    // {
    //     var firstGoal = user.Goals.FirstOrDefault();
    //     
    //     if (firstGoal != null)
    //     {
    //         var goalSetter = new Achievement
    //         {
    //             Name = "Goal Setter",
    //             Description = "You've created your first goal!"
    //         };
    //         if (user.Achievements.Any(a => a.Name != firstGoal.Name)) user.Achievements.Add(goalSetter);
    //         if (_context.Achievements.Any(a => a.Id != goalSetter.Id)) _context.Achievements.Add(goalSetter);
    //         await _context.SaveChangesAsync();
    //     }
    // }
    //
    // public async Task GoalVisionary(ApplicationUser user)
    // {
    //     var completedGoals = user.Goals.Where(goal => goal.Completed).ToList().Count;
    //
    //     switch (completedGoals)
    //     {
    //         case 1:
    //             var goalGetter = new Achievement
    //             {
    //                 Name = "Goal Getter",
    //                 Description = "You've completed your first goal!"
    //             };
    //             if (user.Achievements.Any(a => a.Name != goalGetter.Name)) user.Achievements.Add(goalGetter);
    //             if (_context.Achievements.Any(a => a.Id != goalGetter.Id)) _context.Achievements.Add(goalGetter);
    //             await _context.SaveChangesAsync();
    //             break;
    //         case 3:
    //             var goalDigger = new Achievement
    //             {
    //                 Name = "Goal Digger",
    //                 Description = "You've completed 3 goals!"
    //             };
    //             if (user.Achievements.Any(a => a.Name != goalDigger.Name)) user.Achievements.Add(goalDigger);
    //             if (_context.Achievements.Any(a => a.Id != goalDigger.Id)) _context.Achievements.Add(goalDigger);
    //             await _context.SaveChangesAsync();
    //             break;
    //         case 5:
    //             var goalCrusher = new Achievement
    //             {
    //                 Name = "Goal Crusher",
    //                 Description = "You've completed 5 goals!"
    //             };
    //             if (user.Achievements.Any(a => a.Name != goalCrusher.Name)) user.Achievements.Add(goalCrusher);
    //             if (_context.Achievements.Any(a => a.Id != goalCrusher.Id)) _context.Achievements.Add(goalCrusher);
    //             await _context.SaveChangesAsync();
    //             break;
    //     }
    // }

    public async Task CategorizeAchievement(ApplicationUser user)
    {
        var numberOfCategories = user.Accounts.SelectMany(account => account.Transactions).Select(transaction => transaction.Tag).Distinct().ToList().Count;
        var allCategories = Enum.GetValues(typeof(TransactionCategoryTag)).Length;
        
        switch (numberOfCategories)
        {
            case 1:
                var oneTrickPony = new Achievement
                {
                    Name = "One Trick Pony",
                    Description = "You've only used one category!"
                };
                if (user.Achievements.Any(a => a.Name != oneTrickPony.Name)) user.Achievements.Add(oneTrickPony);
                if (_context.Achievements.Any(a => a.Id != oneTrickPony.Id)) _context.Achievements.Add(oneTrickPony);
                await _context.SaveChangesAsync();
                break;
            case 10:
                var jackOfAllTrades = new Achievement
                {
                    Name = "Jack of All Trades",
                    Description = "You've used 10 categories!"
                };
                if (user.Achievements.Any(a => a.Name != jackOfAllTrades.Name)) user.Achievements.Add(jackOfAllTrades);
                if (_context.Achievements.Any(a => a.Name != jackOfAllTrades.Name)) _context.Achievements.Add(jackOfAllTrades);
                await _context.SaveChangesAsync();
                break;
            case var _ when numberOfCategories == allCategories:
                var masterOfAll = new Achievement
                {
                    Name = "Master of All",
                    Description = "You've used ALL categories!"
                };
                if (user.Achievements.Any(a => a.Name != masterOfAll.Name));
                if (_context.Achievements.Any(a => a.Name != masterOfAll.Name));
                await _context.SaveChangesAsync();
                break;
        }
    }
}