using System.Text.Json.Serialization;

namespace BudgetBuddy.Model;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FinancialRecordTag
{
    Bill,
    Shopping,
    Entertainment,
    Transportation,
    Travel,
    Hospitality,
    Income,
    Transfer,
    Other
}