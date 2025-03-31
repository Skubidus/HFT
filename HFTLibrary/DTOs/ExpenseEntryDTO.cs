using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.DTOs;

public record ExpenseEntryDTO
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; init; }

    [Range(0, 99999999.99)]
    public required decimal Price { get; init; }

    [StringLength(500)]
    public string Description { get; init; } = string.Empty;
    public BankAccountDTO? AssociatedBankAccount { get; init; }
    public required DateTime DateCreated { get; init; }
    public required DateTime DateModified { get; init; }
}
