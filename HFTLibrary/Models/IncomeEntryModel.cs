using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HFTLibrary.Models;

public class IncomeEntryModel
{
    public int Id { get; set; }
    [StringLength(100, MinimumLength = 5)]
    public required string Name { get; set; }
    public required decimal TotalAmount { get; set; }
    [NotMapped]
    public string TotalAmountString
    {
        get
        {
            return TotalAmount.ToString("#,#0.00");
        }

        set
        {
            if (decimal.TryParse(value, out decimal amount) == false)
            {
                return;
            }

            TotalAmount = amount;
        }
    }
    public string Description { get; set; } = string.Empty;
    public required DateTime DateCreated { get; set; }
    public required DateTime DateModified { get; set; }
}