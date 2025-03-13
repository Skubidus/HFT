using HFTLibrary.Models;

namespace HFTLibrary.Data;

public interface IEFCoreData
{
    Task<List<FinancialPlanModel>> GetFinancialPlanListAsync();
    Task<List<(int Id, string Name)>> GetFinancialPlanListLazyAsync();
    Task<FinancialPlanModel?> GetFinancialPlanAsync(int id);
    Task<FinancialPlanModel?> GetFinancialPlanLazyAsync(int id);
    Task CreateFinancialPlanAsync(FinancialPlanModel plan);
    Task DeleteFinancialPlanAsync(int id);
    Task UpdateFinancialPlanAsync(FinancialPlanModel plan);
    Task<ExpenseEntryModel?> GetExpenseEntryAsync(int id);
    Task UpdateExpenseAsync(ExpenseEntryModel entry);
}