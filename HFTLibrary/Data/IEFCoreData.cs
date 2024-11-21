using HFTLibrary.Models;

namespace HFTLibrary.Data;

public interface IEFCoreData
{
    Task<List<(int Id, string Name)>> GetShortFinancialPlanListAsync();
    Task<List<FinancialPlanModel>> GetFullFinancialPlanListAsync();
    Task<FinancialPlanModel?> GetFinancialPlanAsync(int id);
    Task CreateFinancialPlanAsync(FinancialPlanModel plan);
    Task DeleteFinancialPlanAsync(int id);
}