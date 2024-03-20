using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Contracts.Registration;

public record RegistrationRequest(
    [Required]string Email,
    [Required]string Username,
    [Required]string Password);