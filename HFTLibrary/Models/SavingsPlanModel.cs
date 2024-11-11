namespace HFTLibrary.Models;

public abstract class SavingsPlanModel
{
    public List<SavingsEntryModel> SavingsEntries { get; init; } = [];
    public decimal SavingsTotal => SavingsEntries.Sum(x => x.Price);
}