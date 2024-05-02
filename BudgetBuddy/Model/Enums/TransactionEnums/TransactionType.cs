namespace BudgetBuddy.Model.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    Income,
    Expense
}