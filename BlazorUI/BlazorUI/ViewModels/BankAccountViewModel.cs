using System.ComponentModel.DataAnnotations;

namespace BlazorUI.ViewModels;

public class BankAccountViewModel
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; set; }


    private string _iban = string.Empty;
    [StringLength(50)]
    public required string IBAN
    {
        get
        {
            string output = _iban;
            var length = _iban.Length;
            for (int i = 0; i < length; i += 4)
            {
                output = output.Insert(i, " ");
                i++;
                length++;
            }
            return output;
        }
        set => _iban = value;
    }

    [StringLength(20)]
    public required string BIC { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    public required DateTime DateCreated { get; set; }
    public required DateTime DateModified { get; set; }
}