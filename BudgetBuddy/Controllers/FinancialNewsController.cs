using BudgetBuddy.Services.FinancialNewsService;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

public class FinancialNewsController : ControllerBase
{
    private readonly IFinancialNewsProvider _financialNewsProvider;
    
    public FinancialNewsController(IFinancialNewsProvider financialNewsProvider)
    {
        _financialNewsProvider = financialNewsProvider;
    }
    
    [HttpGet("news")]
    public async Task<ActionResult<string>> GetFinancialNews()
    {
        return Ok(await _financialNewsProvider.GetFinancialNews());
    }
}