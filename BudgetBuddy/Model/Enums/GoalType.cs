namespace BudgetBuddy.Model.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GoalType
{
    Saving,
    Spending,
    DebtRepayment,
    Investment,
    Income,
    CharitableGiving
}