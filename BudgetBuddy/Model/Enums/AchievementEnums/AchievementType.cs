using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace BudgetBuddy.Model.Enums.AchievementEnums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AchievementType
{
    Exploration,
    AmountBased
}