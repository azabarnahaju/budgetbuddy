﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.Model;

public class ApplicationUser : IdentityUser
{
    public virtual List<Account> Accounts { get; set; }
    public virtual List<Achievement> Achievements { get; set; }
}