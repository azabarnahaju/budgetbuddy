using BudgetBuddy.Model.Enums.AchievementEnums;
using BudgetBuddy.Model.Enums.TransactionEnums;

namespace BudgetBuddy.Contracts.ModelRequest.UpdateModels;

public record AchievementUpdateRequest(int Id, string Name, AchievementType Type, int Criteria, AchievementObjectiveType Objective, string Description, 
    TransactionType? TransactionType = null, TransactionCategoryTag? TransactionTag = null);