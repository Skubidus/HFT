using System.ComponentModel.DataAnnotations;

namespace BlazorUI.ViewModels;

public class SavingsPlanViewModel
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    public int DurationInMonths { get; set; }
    public List<SavingsEntryViewModel> SavingsEntries { get; init; } = [];
    public required DateTime DateCreated { get; set; }
    public required DateTime DateModified { get; set; }
}