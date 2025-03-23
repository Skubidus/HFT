using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.DTOs;
public record BankAccountDTO
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; init; }

    [StringLength(50)]
    public required string IBAN { get; init; }

    [StringLength(20)]
    public required string BIC { get; init; }

    [StringLength(500)]
    public string Description { get; init; } = string.Empty;
    public required DateTime DateCreated { get; init; }
    public required DateTime DateModified { get; init; }
}