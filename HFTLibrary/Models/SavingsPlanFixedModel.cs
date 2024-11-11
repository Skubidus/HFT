namespace HFTLibrary.Models;

public class SavingsPlanFixedModel : SavingsPlanModel
{
    public int DurationInMonths { get; set; }
    public decimal SavingsRatePerMonth => base.SavingsTotal / DurationInMonths;
}