using System.ComponentModel.DataAnnotations;

namespace HFTLibrary.Models;

public class BankAccountModel
{
    public int Id { get; set; }

    [StringLength(100)]
    public required string BankName { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [StringLength (50)]
    public required string IBAN { get; set; }

    [StringLength(20)]
    public required string BIC { get; set; }

    public required DateTime DateCreated { get; set; }
    public required DateTime DateModified { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null
            || obj is not BankAccountModel)
        {
            return false;
        }

        var model = obj as BankAccountModel;

        return model!.Id == this.Id;
    }
}