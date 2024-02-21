using BudgetBuddy.Model;

namespace BudgetBuddy.Services.Repositories.FinancialRecord;
using BudgetBuddy.Model.Record;

public interface IFinancialRecordRepository
{
    IEnumerable<FinancialRecord> GetAllRecords();
    FinancialRecord GetRecord(int id);
    void AddRecord(FinancialRecord record);
    FinancialRecord UpdateRecord(FinancialRecord record);
    void DeleteRecord(int id);

    IEnumerable<FinancialRecord> FilterRecords(RecordType recordType);
    IEnumerable<FinancialRecord> FinancialRecords(FinancialRecordTag tag);
}