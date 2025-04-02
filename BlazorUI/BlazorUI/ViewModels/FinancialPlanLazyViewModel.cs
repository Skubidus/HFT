using System.ComponentModel.DataAnnotations;

namespace BlazorUI.ViewModels;

public class FinancialPlanLazyViewModel
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; set; }
}
