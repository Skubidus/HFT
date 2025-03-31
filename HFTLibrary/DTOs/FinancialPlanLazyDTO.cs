using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.DTOs;

public record FinancialPlanLazyDTO
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; init; }
}