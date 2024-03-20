using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Contracts;

public record RegistrationResponse(
    string Email,
    string Username);