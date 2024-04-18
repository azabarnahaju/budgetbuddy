using BudgetBuddy.Data;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services.AchievementService;

public class AchievementService : IAchievementService
{
    
    private readonly BudgetBuddyContext _context;
    
    public AchievementService(BudgetBuddyContext context)
    {
        _context = context;
    }
    public async Task UpdateAccountAchievements(ApplicationUser user)
    {
        await AccountAchievement(user);
    }

    public async Task UpdateTransactionAchievements(ApplicationUser user)
    { 
        await ExpenseTransactionTrackerAchievement(user);
        await IncomeTransactionTrackerAchievement(user);
        await SavingsAchievement(user);
        await CategorizeAchievement(user);
    }
    
    public async Task UpdateGoalAchievements(ApplicationUser user)
    {
        await GoalAchievement(user);
        await CompletedGoalAchievement(user);
    }

    // public async Task BudgetMasterAchievement(ApplicationUser user, decimal budgetAmount)
    // {
    //     var appUser = await _context.Users.Include(applicationUser => applicationUser.Achievements).FirstOrDefaultAsync(applicationUser => true);
    //     var transactionSum = _context.Users.Where(u => u.Id == user.Id)
    //         .SelectMany(account => account.Accounts)
    //         .SelectMany(account => account.Transactions)
    //         .Where(t => t.Type == TransactionType.Expense)
    //         .Where(t => t.Date >= DateTime.Now.AddDays(-30))
    //         .Sum(t => t.Amount);
    //     
    //     Console.WriteLine(user.Accounts.SelectMany(account => account.Transactions));
    //         var transactions = user.Accounts.SelectMany(account => account.Transactions)
    //             .Where(transaction => transaction.Date >= DateTime.Now.AddDays(-30)).ToList()
    //             .Sum(transaction => transaction.Amount);
    //
    //         if (transactionSum < budgetAmount)
    //         {
    //             var budgetMaster = new Achievement
    //             {
    //                 Name = "Budget Master",
    //                 Description = "You've spent less than your budget in the past 30 days!"
    //             };
    //             if (appUser.Achievements.Any(a => a.Name != budgetMaster.Name)) appUser.Achievements.Add(budgetMaster);
    //             if (_context.Achievements.Any(a => a.Id != budgetMaster.Id)) _context.Achievements.Add(budgetMaster);
    //             
    //             await _context.SaveChangesAsync();
    //         }
    // }


    private async Task ExpenseTransactionTrackerAchievement(ApplicationUser applicationUser)
    {
        var user = await _context.Users.Include(u => u.Accounts).ThenInclude(a => a.Transactions)
            .Include(applicationUser => applicationUser.Achievements).FirstOrDefaultAsync(u => u.Id == applicationUser.Id);
        var totalTransactions = user.Accounts.Sum(account => account.Transactions.Where(transaction =>  transaction.Type == TransactionType.Expense).ToList().Count);

        switch (totalTransactions)
        {
            case 1:
                var pioneer = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Pioneer");
                user.Achievements.Add(pioneer);
                await _context.SaveChangesAsync();
                break;
            case 5:
                var bigSpender = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Big Spender");
                if (user.Achievements.Any(a => a.Id != bigSpender.Id)) user.Achievements.Add(bigSpender);
                await _context.SaveChangesAsync();
                break;
            case 10:
                var moneyBags = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Money Bags");
                if (user.Achievements.Any(a => a.Id != moneyBags.Id)) user.Achievements.Add(moneyBags);
                await _context.SaveChangesAsync();
                break;
        }
    }
    
    private async Task IncomeTransactionTrackerAchievement(ApplicationUser applicationUser)
    {
        var user = await _context.Users.Include(u => u.Accounts).ThenInclude(a => a.Transactions)
            .Include(applicationUser => applicationUser.Achievements).FirstOrDefaultAsync(u => u.Id == applicationUser.Id);
        var totalTransactions = user.Accounts.Sum(account => account.Transactions.Where(transaction =>  transaction.Type == TransactionType.Income).ToList().Count);

        switch (totalTransactions)
        {
            case 1:
                var moneyMaker = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Money Maker");
                if (user.Achievements.Any(a => a.Id != moneyMaker.Id)) user.Achievements.Add(moneyMaker);
                await _context.SaveChangesAsync();
                break;
            case 5:
                var wealthBuilder = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Wealth Builder");
                if (user.Achievements.Any(a => a.Id != wealthBuilder.Id)) user.Achievements.Add(wealthBuilder);
                await _context.SaveChangesAsync();
                break;
            case 10:
                var moneyBags = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Financial Wizard");
                if (user.Achievements.Any(a => a.Id != moneyBags.Id)) user.Achievements.Add(moneyBags);
                await _context.SaveChangesAsync();
                break;
        }
    }

    private async Task AccountAchievement(ApplicationUser applicationUser)
    {
        var user = await _context.Users.Include(u => u.Accounts).ThenInclude(a => a.Transactions)
            .Include(applicationUser => applicationUser.Achievements).FirstOrDefaultAsync(u => u.Id == applicationUser.Id);
        
        if (user.Accounts.Count == 1)
        {
            var accountHolder = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Account Holder");
            user.Achievements.Add(accountHolder);
            await _context.SaveChangesAsync();
        }
    }
    
    private async Task SavingsAchievement(ApplicationUser applicationUser)
    {
        var user = await _context.Users.Include(u => u.Accounts).ThenInclude(a => a.Transactions)
            .Include(applicationUser => applicationUser.Achievements).FirstOrDefaultAsync(u => u.Id == applicationUser.Id);
        var totalSavings = user.Accounts.Sum(account => account.Transactions.Where(transaction => transaction.Type == TransactionType.Income).Sum(transaction => transaction.Amount));

        if (totalSavings >= 1500 && user.Achievements.All(a => a.Name != "Thrifty"))
        {
            var thrifty = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Thrifty");
            user.Achievements.Add(thrifty);
            await _context.SaveChangesAsync();
        }
        else if (totalSavings >= 1000 && user.Achievements.All(a => a.Name != "Frugal"))
        {
            var frugal = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Frugal");
            user.Achievements.Add(frugal);
            await _context.SaveChangesAsync();
        }
        else if (totalSavings >= 500 && user.Achievements.All(a => a.Name != "Penny Pincher"))
        {
            var pennyPincher = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Penny Pincher");
            user.Achievements.Add(pennyPincher);
            await _context.SaveChangesAsync();
        }
    }

    private async Task GoalAchievement(ApplicationUser applicationUser)
    {
        var user = await _context.Users.Include(u => u.Accounts).ThenInclude(a => a.Transactions)
            .Include(applicationUser => applicationUser.Achievements).FirstOrDefaultAsync(u => u.Id == applicationUser.Id);
        var goals = _context.Goals.Count(goal => goal.UserId == user.Id);

        switch (goals)
        {
            case 1:
                var goalSetter = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Goal Setter");
                user.Achievements.Add(goalSetter);
                await _context.SaveChangesAsync();
                break;
            case 3:
                var goalAchiever = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Goal Achiever");
                user.Achievements.Add(goalAchiever);
                await _context.SaveChangesAsync();
                break;
            case 5:
                var masterOfGoals = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Master of Goals");
                user.Achievements.Add(masterOfGoals);
                await _context.SaveChangesAsync();
                break;
        }
    }
    
    private async Task CompletedGoalAchievement(ApplicationUser applicationUser)
    {
        var user = await _context.Users.Include(u => u.Accounts).ThenInclude(a => a.Transactions)
            .Include(applicationUser => applicationUser.Achievements).FirstOrDefaultAsync(u => u.Id == applicationUser.Id);
        
        var completedGoals = _context.Goals.Where(goal => goal.UserId == user.Id).Count(goal => goal.Completed);
    
        switch (completedGoals)
        {
            case 1:
                var goalGetter = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Goal Getter");
                user.Achievements.Add(goalGetter);
                await _context.SaveChangesAsync();
                break;
            case 3:
                var goalDigger = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Goal Digger");
                user.Achievements.Add(goalDigger);
                await _context.SaveChangesAsync();
                break;
            case 5:
                var goalCrusher = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Goal Crusher");
                user.Achievements.Add(goalCrusher);
                await _context.SaveChangesAsync();
                break;
        }
    }

    private async Task CategorizeAchievement(ApplicationUser applicationUser)
    {
        var user = await _context.Users.Include(u => u.Accounts).ThenInclude(a => a.Transactions)
            .Include(applicationUser => applicationUser.Achievements).FirstOrDefaultAsync(u => u.Id == applicationUser.Id);
        var numberOfCategories = user.Accounts.SelectMany(account => account.Transactions).Select(transaction => transaction.Tag).Distinct().ToList().Count;
        var allCategories = Enum.GetValues(typeof(TransactionCategoryTag)).Length;
        
        switch (numberOfCategories)
        {
            case 5:
                var fiveStarDabbler = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Five-Star Dabbler");
                user.Achievements.Add(fiveStarDabbler);
                await _context.SaveChangesAsync();
                break;
            case 10:
                var jackOfAllTrades = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Jack of All Trades");
                user.Achievements.Add(jackOfAllTrades);
                await _context.SaveChangesAsync();
                break;
            case var _ when numberOfCategories == allCategories:
                var masterOfAll = await _context.Achievements.FirstOrDefaultAsync(a => a.Name == "Master of All");
                user.Achievements.Add(masterOfAll);
                await _context.SaveChangesAsync();
                break;
        }
    }
}