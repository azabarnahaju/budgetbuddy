using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.AchievementEnums;
using BudgetBuddy.Model.Enums.TransactionEnums;

namespace BudgetBuddy.Contracts.ModelRequest.CreateModels;

public record AchievementCreateRequest(string Name, AchievementType Type, int Criteria, AchievementObjectiveType Objective, 
    TransactionType? TransactionType = null, TransactionCategoryTag? TransactionTag = null);