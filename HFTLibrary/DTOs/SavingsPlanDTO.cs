using HFTLibrary.Models;

using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.DTOs;

public record SavingsPlanDTO
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; init; }

    [StringLength(500)]
    public string Description { get; init; } = string.Empty;
    public int DurationInMonths { get; init; }
    public IReadOnlyList<SavingsEntryDTO> SavingsEntries { get; init; } = Array.Empty<SavingsEntryDTO>();
    public required DateTime DateCreated { get; init; }
    public required DateTime DateModified { get; init; }
}