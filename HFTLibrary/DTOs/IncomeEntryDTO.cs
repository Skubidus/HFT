using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.DTOs;

public record IncomeEntryDTO
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; init; }

    [StringLength(500)]
    public string Description { get; init; } = string.Empty;
    public required decimal TotalAmount { get; init; }
    public required DateTime DateCreated { get; init; }
    public required DateTime DateModified { get; init; }
}