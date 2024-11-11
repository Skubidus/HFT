namespace HFTLibrary.Models;

public class IncomeEntryModel
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required decimal Price { get; set; }
    public BankAccountModel? AssociatedBankAccount { get; set; }
    public string Note { get; set; } = string.Empty;
}