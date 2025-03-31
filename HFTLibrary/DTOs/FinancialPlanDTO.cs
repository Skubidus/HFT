using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.DTOs;
public record FinancialPlanDTO
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; init; }

    [StringLength(500)]
    public string Description { get; init; } = string.Empty;
    public IReadOnlyList<IncomeEntryDTO> Incomes { get; init; } = Array.Empty<IncomeEntryDTO>();
    public IReadOnlyList<ExpenseEntryDTO> Expenses { get; init; } = Array.Empty<ExpenseEntryDTO>();
    public IReadOnlyList<BankAccountDTO> BankAccounts { get; init; } = Array.Empty<BankAccountDTO>();
    public SavingsPlanDTO? SavingsPlan { get; init; } = null;
    public required DateTime DateCreated { get; init; }
    public required DateTime DateModified { get; init; }
}