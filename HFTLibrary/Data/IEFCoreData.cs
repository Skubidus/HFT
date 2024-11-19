namespace HFTLibrary.Data;

public interface IEFCoreData
{
    Task<List<(int Id, string Name)>> GetFinancialPlanListAsync();
}