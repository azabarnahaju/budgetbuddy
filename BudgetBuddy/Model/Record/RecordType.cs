using System.Text.Json.Serialization;
namespace BudgetBuddy.Model.Record;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RecordType
{
    Expense,
    Income
}