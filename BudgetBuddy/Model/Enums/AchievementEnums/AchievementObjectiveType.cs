using System.Text.Json.Serialization;

namespace BudgetBuddy.Model.Enums.AchievementEnums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AchievementObjectiveType
{
    Goal,
    Report,
    Account,
    Transaction,
    TransactionTag,
    TransactionType
}