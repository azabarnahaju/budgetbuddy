namespace BudgetBuddy.Services.Repositories.Report;
using BudgetBuddy.Model;

public interface IReportRepository
{
    Task<IEnumerable<Report>> GetAllReports();
    Task<IEnumerable<Report>> GetReportsByAccount(int accountId);
    Task<IEnumerable<Report>> GetReportsByUser(string userId);
    Task<Report> GetReport(int id);
    Task<Report> AddReport(Report report);
    Task DeleteReport(int id);
}