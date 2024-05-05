using System.Text.Json.Serialization;

namespace BudgetBuddy.Model.Enums.TransactionEnums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    Income,
    Expense
}