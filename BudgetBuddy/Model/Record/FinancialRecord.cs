namespace BudgetBuddy.Model.Record;

public class FinancialRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Name { get; set; }
    public FinancialRecordTag Tag { get; set; }
    public RecordType Type { get; set; }
    public int AccountId { get; set; }
}