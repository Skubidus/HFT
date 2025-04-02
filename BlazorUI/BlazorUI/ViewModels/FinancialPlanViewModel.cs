using System.ComponentModel.DataAnnotations;

namespace BlazorUI.ViewModels;

public class FinancialPlanViewModel
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    public List<IncomeEntryViewModel> Incomes { get; init; } = [];
    public List<ExpenseEntryViewModel> Expenses { get; init; } = [];
    public List<BankAccountViewModel> BankAccounts { get; init; } = [];
    public SavingsPlanViewModel? SavingsPlan { get; set; } = null;
    public required DateTime DateCreated { get; set; }
    public required DateTime DateModified { get; set; }
}