namespace HFTLibrary.Models;

public class ExpenseEntryModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
    public BankAccountModel? AssociatedBankAccount { get; set; }
    public string Note { get; set; } = string.Empty;
}