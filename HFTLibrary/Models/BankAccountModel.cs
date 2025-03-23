using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.Models;

public class BankAccountModel
{
    public int Id { get; set; }

    [StringLength(100)]
    public required string Name { get; set; }

    [StringLength (50)]
    public required string IBAN { get; set; }

    [StringLength(20)]
    public required string BIC { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    public required DateTime DateCreated { get; set; }
    public required DateTime DateModified { get; set; }
}