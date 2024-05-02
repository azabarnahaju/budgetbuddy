using System.Text.Json.Serialization;

namespace BudgetBuddy.Model.Enums.AchievementEnums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AchievementObjectiveType
{
    Feature,
    Goal,
    Report,
    Account,
    Transaction,
    TransactionTag,
    TransactionType
}