namespace BudgetBuddy.Model.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionCategoryTag
{
    Food,
    Groceries,
    DiningOut,
    Transportation,
    Housing,
    Utilities,
    Entertainment,
    DeptRepayment,
    Investment,
    Clothing,
    HealthCare,
    Education,
    Electronics,
    PersonalCare,
    Travel,
    Insurance,
    Income,
    Gifts,
    CharitableDonations,
    PetCare,
    HomeImprovement,
    Subscriptions,
    Other
}