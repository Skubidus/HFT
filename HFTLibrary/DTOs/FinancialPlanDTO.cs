using HFTLibrary.Models;

using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.DTOs;
public record FinancialPlanDTO
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; init; }

    [StringLength(500)]
    public string Description { get; init; } = string.Empty;
    public List<IncomeEntryModel> Incomes { get; init; } = [];
    public List<ExpenseEntryModel> Expenses { get; init; } = [];
    public List<BankAccountModel> BankAccounts { get; init; } = [];
    public int? SavingsPlanId { get; init; } = null;
    public required DateTime DateCreated { get; init; }
    public required DateTime DateModified { get; init; }
}