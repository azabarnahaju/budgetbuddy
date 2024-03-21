using BudgetBuddy.Contracts.ModelRequest;
using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Services.ReportServices;
using BudgetBuddy.Services.Repositories.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase
{
    private readonly ILogger<ReportController> _logger;
    private readonly IReportRepository _reportRepository;
    private readonly IReportService _reportService;

    public ReportController(ILogger<ReportController> logger, IReportRepository reportRepository, IReportService reportService)
    {
        _logger = logger;
        _reportRepository = reportRepository;
        _reportService = reportService;
    }

    [HttpGet, Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<Report>>> GetAll()
    {
        try
        {
            return Ok(new { message = "Reports found successfully.", data = await _reportRepository.GetAllReports() });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't find reports.");
            return NotFound(new { message = "Can't find reports." });
        }
    }

    [HttpGet("{reportId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Report>> GetById(int reportId)
    {
        try
        {
            return Ok(new
            {
                message = "Report found successfully.", 
                data = await _reportRepository.GetReport(reportId)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Report couldn't be found.");
            return NotFound(new { message = e.Message });
        }    
    }

    [HttpGet("Report/user/{userId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<IEnumerable<Report>>> GetByUserId(string userId)
    {
        try
        {
            return Ok(new
            {
                message = "Reports found successfully.", 
                data = await _reportRepository.GetReportsByUser(userId)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Reports couldn't be found.");
            return NotFound(new { message = e.Message });
        }
    }
    
    [HttpGet("report/account/{accountId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<IEnumerable<Report>>> GetByAccountId(int accountId)
    {
        try
        {
            return Ok(new
            {
                message = "Reports found successfully.", 
                data = await _reportRepository.GetReportsByAccount(accountId)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Reports couldn't be found.");
            return NotFound(new { message = e.Message });
        }
    }
    
    
    [HttpPost("Add"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Report>> Add(ReportCreateRequest createRequest)
    {
        try
        {
            var report = await _reportService.CreateReport(createRequest);
            return Ok(new
            {
                message = "Report successfully created and added.",
                data = await _reportRepository.AddReport(report)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating and adding report.");
            return NotFound(new { message = e.Message });
        }
    }
    
    [HttpDelete("{reportId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<string>> Delete(int reportId)
    {
        try
        {
            await _reportRepository.DeleteReport(reportId);
            return Ok(new { message = "Deleting report was successful." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Deleting report has failed.");
            return BadRequest(new { message = e.Message });
        }
    }
}