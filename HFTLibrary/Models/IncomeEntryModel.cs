using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.Models;

public class IncomeEntryModel
{
    public int Id { get; set; }

    [StringLength(100)]
    public required string Name { get; set; }
    public required decimal TotalAmount { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    public required DateTime DateCreated { get; set; }
    public required DateTime DateModified { get; set; }
}