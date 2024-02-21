using BudgetBuddy.Model;

namespace BudgetBuddy.Services.Repositories.FinancialRecord;
using Model.Record;

public class FinancialRecordRepository : IFinancialRecordRepository
{
    private IList<FinancialRecord> _records = new List<FinancialRecord>();
    private ILogger<FinancialRecordRepository> _logger;

    public FinancialRecordRepository(ILogger<FinancialRecordRepository> logger)
    {
        _logger = logger;
    }

    public IEnumerable<FinancialRecord> GetAllRecords()
    {
        _logger.LogInformation("Getting all records.");
        return _records;
    }
    
    public FinancialRecord GetRecord(int id)
    {
        
        return _records.All(record => record.Id != id) ? throw new Exception("Record not found") : _records.First(record => record.Id == id);
    }

    public void AddRecord(FinancialRecord record)
    {
        if (_records.Any(r => r.Id == record.Id)) throw new Exception("Record already exists.");

        _records.Add(record);
        _logger.LogInformation("Record added.");
    }

    public FinancialRecord UpdateRecord(FinancialRecord record)
    {
        if(_records.FirstOrDefault(r => r.Id == record.Id) is null ) throw new Exception("Record not found.");
        
        _records = _records.Select(r => record.Id == r.Id ? record : r).ToList();
        return _records.First(r => r.Id == record.Id);
    }

    public void DeleteRecord(int id)
    {
        if (_records.All(r => r.Id != id)) throw new Exception("Record is not found by ID.");

        _records = _records.Where(r => r.Id != id).ToList();
    }

    public IEnumerable<FinancialRecord> FilterRecords(RecordType recordType)
    {
        if(_records.All(r => r.Type != recordType)) throw new Exception("No record found by type.");
        
        return _records.Where(r => r.Type == recordType);
    }

    public IEnumerable<FinancialRecord> FinancialRecords(FinancialRecordTag tag)
    {
        if(_records.All(r => r.Tag != tag)) throw new Exception($"No record found by {tag.ToString()}");
        
        return _records.Where(r => r.Tag == tag);
    }
}