using System.ComponentModel.DataAnnotations;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Record;
using BudgetBuddy.Services.Repositories.FinancialRecord;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;
[ApiController]
[Route("[controller]")]
public class FinancialRecordController : ControllerBase
{
    private readonly IFinancialRecordRepository _financialRecordRepository;
    private readonly ILogger<FinancialRecordController> _logger;
    
    public FinancialRecordController(ILogger<FinancialRecordController> logger, IFinancialRecordRepository financialRecordRepository)
    {
        _logger = logger;
        _financialRecordRepository = financialRecordRepository;
    }
    
    [HttpPost("add")]
    public ActionResult AddRecord(FinancialRecord record)
    {
        try
        {
            _financialRecordRepository.AddRecord(record);
            return Ok(new { message = "FinancialRecord added.", data = record });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Record already exists.");
            return NotFound("Record already exists.");
        }
    }

    [HttpGet("records")]
    public ActionResult GetAll()
    {
        try
        {
            return Ok(new { message = "Records retrieved.", data = _financialRecordRepository.GetAllRecords() });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting records");
            return NotFound("Error getting records");
        }
    }
    [HttpGet("records/{id}")]
    public ActionResult<FinancialRecord> GetRecord(int id)
    {
        try
        {
            return Ok(new { message = "Record retrieved.", data = _financialRecordRepository.GetRecord(id) });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Record not found.");
            return NotFound("Record not found.");
        }
    }
    
    [HttpPatch("update")]
    public ActionResult UpdateRecord(FinancialRecord record)
    {
        try
        {
            return Ok(new { message = "Record updated.", data = _financialRecordRepository.UpdateRecord(record) });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating record");
            return NotFound("Error updating record");
        }
    }
    
    [HttpDelete("delete/{id}")]
    public ActionResult DeleteRecord(int id)
    {
        try
        {
            _financialRecordRepository.DeleteRecord(id);
            return Ok(new { message = "Record deleted." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting record");
            return NotFound("Error deleting record");
        }
    }

    [HttpGet("filter/{recordType}")]
    public ActionResult FilterRecords([Required]RecordType recordType)
    {
        try
        {
            return Ok(new { message = $"Records filtered by {recordType}.", data = _financialRecordRepository.FilterRecords(recordType) });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error filtering records by {recordType}");
            return NotFound($"Error filtering records by {recordType}");
        }
    }
    
    [HttpGet("filter/{tag}")]
    public ActionResult FinancialRecords([Required]FinancialRecordTag tag)
    {
        try
        {
            return Ok(new { message = $"Records filtered by {tag}.", data = _financialRecordRepository.FinancialRecords(tag) });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error filtering records by {tag} tag.");
            return NotFound($"Error filtering records by {tag} tag.");
        }
    }
}