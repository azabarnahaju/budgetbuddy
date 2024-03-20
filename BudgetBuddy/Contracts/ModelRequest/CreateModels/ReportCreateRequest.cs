using BudgetBuddy.Model.Enums;

namespace BudgetBuddy.Contracts.ModelRequest;

public record ReportCreateRequest(int AccountId, ReportType ReportType, DateTime? StartDate = null, DateTime? EndDate = null);