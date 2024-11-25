using System.ComponentModel.DataAnnotations.Schema;

namespace HFTLibrary.Models;

public class SavingsEntryModel
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public required decimal Price { get; set; }
    [NotMapped]
    public string PriceString
    {
        get
        {
            return Price.ToString("#,#0.00");
        }

        set
        {
            if (decimal.TryParse(value, out decimal price) == false)
            {
                return;
            }

            Price = price;
        }
    }
    public string Description { get; set; } = string.Empty;
	public required DateTime DateCreated { get; set; }
	public required DateTime DateModified { get; set; }
}