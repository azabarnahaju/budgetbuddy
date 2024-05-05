using System.Collections;
using System.Net.Mime;
using System.Numerics;
using BudgetBuddy.Data;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.AchievementEnums;
using BudgetBuddy.Services.Repositories.Achievement;
using BudgetBuddy.Services.Repositories.Goal;
using BudgetBuddy.Services.Repositories.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BudgetBuddy.Services.AchievementService;

public class AchievementService : IAchievementService
{

    private readonly IUserRepository _userRepository;
    private readonly IAchievementRepository _achievementRepository;
    private readonly IGoalRepository _goalRepository;
    
    public AchievementService(IUserRepository userRepository, IAchievementRepository achievementRepository, IGoalRepository goalRepository)
    {
        _userRepository = userRepository;
        _achievementRepository = achievementRepository;
        _goalRepository = goalRepository;
    }
    
    public async Task UpdateGoalAchievements(ApplicationUser user)
    {
        var userGoalsCount = user.Accounts.Select(a =>a.Goals).SelectMany(g => g).Count();
        var goalAchievements = await _achievementRepository.GetAchievementsByObjective(AchievementObjectiveType.Goal);
        foreach (var achievement in goalAchievements)
        {
            if (userGoalsCount < achievement.Criteria) continue;
            await _userRepository.AddAchievementToUser(user.Id, achievement);
        }
    }

    public async Task UpdateRecordAchievements(ApplicationUser user)
    {
        var userReportsCount = user.Accounts.Select(a => a.Reports.Count).Sum();
        var reportAchievements =
            await _achievementRepository.GetAchievementsByObjective(AchievementObjectiveType.Report);
        foreach (var achievement in reportAchievements)
        {
            if (userReportsCount < achievement.Criteria) continue;
            await _userRepository.AddAchievementToUser(user.Id, achievement);
        }
    }

    public async Task UpdateAccountAchievements(ApplicationUser user)
    {
        var userAccountCount = user.Accounts.Count;
        var userAccountAmount = user.Accounts.Select(a => a.Balance).Sum();
        
        var accountAchievements = await _achievementRepository.GetAchievementsByObjective(AchievementObjectiveType.Account);
        var explorationAchievements = accountAchievements.Where(a => a.Type == AchievementType.Exploration);
        var amountAchievements = accountAchievements.Where(a => a.Type == AchievementType.AmountBased);
        
        await CheckAndAddAchievement(userAccountAmount, userAccountCount, explorationAchievements, amountAchievements, user.Id);
    }

    public async Task UpdateTransactionAchievements(ApplicationUser user)
    {
        var userTransactions = user.Accounts.Select(a => a.Transactions).SelectMany(t => t);
        await UpdateCoreTransactionAchievements(user, userTransactions);
        await UpdateTransactionTypeAchievements(user, userTransactions);
        await UpdateTransactionTagAchievements(user, userTransactions);
    }

    private async Task UpdateCoreTransactionAchievements(ApplicationUser user, IEnumerable<Transaction> transactions)
    {
        var userTransactionCount = transactions.Count();
        
        var transactionAchievements = await _achievementRepository.GetAchievementsByObjective(AchievementObjectiveType.Transaction);
        var explorationAchievements = transactionAchievements.Where(a => a is { Type: AchievementType.Exploration, TransactionType: null, TransactionTag: null });
        
        foreach (var achievement in explorationAchievements)
        {
            if (userTransactionCount < achievement.Criteria) continue;
            await _userRepository.AddAchievementToUser(user.Id, achievement);
        }
    }

    private async Task UpdateTransactionTypeAchievements(ApplicationUser user, IEnumerable<Transaction> transactions)
    {
        var groupedTransactions = transactions.GroupBy(t => t.Type);
        var typeAchievements =
            await _achievementRepository.GetAchievementsByObjective(AchievementObjectiveType.TransactionType);

        foreach (var group in groupedTransactions)
        {
            var transactionCount = group.Count();
            var transactionAmount = group.Sum(t => t.Amount);
            var achievementsWithTypeExploration = typeAchievements.Where(a => a.TransactionType == group.Key && a.Type == AchievementType.Exploration);
            var achievementsWithTypeAmount = typeAchievements.Where(a => a.TransactionType == group.Key && a.Type == AchievementType.AmountBased);
            
            await CheckAndAddAchievement(transactionAmount, transactionCount, achievementsWithTypeExploration, achievementsWithTypeAmount, user.Id);
        }
    }

    private async Task UpdateTransactionTagAchievements(ApplicationUser user, IEnumerable<Transaction> transactions)
    {
        var groupedTransactions = transactions.GroupBy(t => t.Tag);
        var tagAchievements =
            await _achievementRepository.GetAchievementsByObjective(AchievementObjectiveType.TransactionTag);

        foreach (var group in groupedTransactions)
        {
            var transactionCount = group.Count();
            var transactionAmount = group.Sum(t => t.Amount);
            var achievementsWithTagExploration = tagAchievements.Where(a => a.TransactionTag == group.Key && a.Type == AchievementType.Exploration);
            var achievementsWithTagAmount = tagAchievements.Where(a => a.TransactionTag == group.Key && a.Type == AchievementType.AmountBased);

            await CheckAndAddAchievement(transactionAmount, transactionCount, achievementsWithTagExploration, achievementsWithTagAmount, user.Id);
        }
    }

    private async Task CheckAndAddAchievement(decimal sum, int count, IEnumerable<Achievement> explorationAchievements, IEnumerable<Achievement> amountBasedAchievements, string userId)
    {
        foreach (var achievement in explorationAchievements)
        {
            if (count < achievement.Criteria) continue;
            await _userRepository.AddAchievementToUser(userId, achievement);
        }
            
        foreach (var achievement in amountBasedAchievements)
        {
            if (sum < achievement.Criteria) continue;
            await _userRepository.AddAchievementToUser(userId, achievement);
        }
    }
    
}