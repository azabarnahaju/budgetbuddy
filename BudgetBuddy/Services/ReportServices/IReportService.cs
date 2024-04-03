using BudgetBuddy.Contracts.ModelRequest;
using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;

namespace BudgetBuddy.Services.ReportServices;

public interface IReportService
{
    Task<Report> CreateReport(ReportCreateRequest createRequest);
}