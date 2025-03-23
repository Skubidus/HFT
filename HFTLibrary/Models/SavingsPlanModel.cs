using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.Models;

public class SavingsPlanModel
{
    public int Id { get; set; }

    [StringLength(100)]
    public required string Name { get; set; }
    public List<SavingsEntryModel> SavingsEntries { get; init; } = [];
	//public decimal SavingsTotal => SavingsEntries.Sum(x => x.Price);
	public int DurationInMonths { get; set; }
    //public decimal SavingsRatePerMonth => SavingsTotal / DurationInMonths;

    [StringLength(500)]
	public string Description { get; set; } = string.Empty;
	public required DateTime DateCreated { get; set; }
	public required DateTime DateModified { get; set; }
}