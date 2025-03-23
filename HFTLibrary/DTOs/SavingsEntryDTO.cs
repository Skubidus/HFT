using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.DTOs;

public record SavingsEntryDTO
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; init; }
    public required decimal Price { get; init; }

    [StringLength(500)]
    public string Description { get; init; } = string.Empty;
    public required DateTime DateCreated { get; init; }
    public required DateTime DateModified { get; init; }
}