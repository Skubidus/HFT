namespace HFTLibrary.Models;

public class FinancialPlanModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<IncomeEntryModel> Incomes { get; init; } = [];
    public List<ExpenseEntryModel> Expenses { get; init; } = [];
    public List<BankAccountModel> BankAccounts { get; init; } = [];
    public SavingsPlanModel? SavingsPlan { get; set; } = null;
    public required DateTime DateCreated { get; set; }
    public required DateTime DateModified { get; set; }
}