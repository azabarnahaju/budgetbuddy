using System.Text.Json.Serialization;

namespace BudgetBuddy.Model.Enums.AchievementEnums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FeatureExplorationAchievementObject
{
    Transaction,
    Goal,
    Account,
    Report
}