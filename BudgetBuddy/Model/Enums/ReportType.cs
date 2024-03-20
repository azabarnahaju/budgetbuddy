using System.Text.Json.Serialization;

namespace BudgetBuddy.Model.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReportType
{
    Weekly,
    Monthly,
    Yearly,
    Last7Days,
    Last30Days,
    Custom
}