namespace HFTLibrary.Models;

public class IncomeEntryModel
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public required decimal TotalAmount { get; set; }
	public BankAccountModel? AssociatedBankAccount { get; set; }
	public string Description { get; set; } = string.Empty;
	public required DateTime DateCreated { get; set; }
	public required DateTime DateModified { get; set; }
}