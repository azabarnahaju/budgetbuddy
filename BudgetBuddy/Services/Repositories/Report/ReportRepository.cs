namespace BudgetBuddy.Services.Repositories.Report;

using Microsoft.EntityFrameworkCore;
using Model;
using Data;

public class ReportRepository : IReportRepository
{
    private readonly BudgetBuddyContext _database;

    public ReportRepository(BudgetBuddyContext database)
    {
        _database = database;
    }

    public async Task<IEnumerable<Report>> GetAllReports()
    {
        return await _database.Reports.ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetReportsByAccount(int accountId)
    {
        if (!await _database.Accounts.AnyAsync(a => a.Id == accountId))
            throw new Exception($"No account exists with ID {accountId}");

        return await _database.Reports.Include(r => r.Account).Where(r => r.AccountId == accountId).ToListAsync();
    }
    
    public async Task<IEnumerable<Report>> GetReportsByUser(string userId)
    {
        if (!await _database.Accounts.AnyAsync(a => a.UserId == userId))
            throw new Exception($"No account exists with User ID {userId}");

        return await _database.Reports.Where(r => r.Account.UserId == userId).ToListAsync();
    }

    public async Task<Report> GetReport(int id)
    {
        if (!await _database.Reports.AnyAsync(r => r.Id == id))
            throw new Exception($"No report exists with ID {id}");

        return await _database.Reports.FirstAsync(r => r.Id == id);
    }

    public async Task<Report> AddReport(Report report)
    {
        if (await _database.Reports.AnyAsync(a => a.Id == report.Id)) 
            throw new Exception($"Report with ID {report.Id} already exists.");
        
        var result = await _database.Reports.AddAsync(report);
        await _database.SaveChangesAsync();

        return result.Entity;
    }

    public async Task DeleteReport(int id)
    {
        if (!await _database.Reports.AnyAsync(a => a.Id == id)) 
            throw new Exception($"Report with ID {id} doesn't exists.");
        
        _database.Reports.Remove(await GetReport(id));
        await _database.SaveChangesAsync();
    }
}