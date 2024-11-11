namespace HFTLibrary.Models;

public class SavingsEntryModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}