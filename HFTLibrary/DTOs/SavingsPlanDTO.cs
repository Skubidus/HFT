using HFTLibrary.Models;

using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.DTOs;

public record SavingsPlanDTO
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; init; }
    public List<SavingsEntryModel> SavingsEntries { get; init; } = [];
    //public decimal SavingsTotal => SavingsEntries.Sum(x => x.Price);
    public int DurationInMonths { get; init; }
    //public decimal SavingsRatePerMonth => SavingsTotal / DurationInMonths;

    [StringLength(500)]
    public string Description { get; init; } = string.Empty;
    public required DateTime DateCreated { get; init; }
    public required DateTime DateModified { get; init; }
}