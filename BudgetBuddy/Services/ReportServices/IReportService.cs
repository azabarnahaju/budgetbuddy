using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;

namespace BudgetBuddy.Services.ReportServices;

public interface IReportService
{
    Task<Report> CreateReport(Account account, ReportType type, DateTime? startDate = null, DateTime? endDate = null);
}