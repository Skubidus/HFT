using HFTLibrary.DTOs;

namespace HFTLibrary.Data;

public interface IEFCoreData
{
    // FinancialPlan
    Task<FinancialPlanDTO?> GetFinancialPlanAsync(int id);
    Task<FinancialPlanLazyDTO?> GetFinancialPlanLazyAsync(int id);
    Task<List<FinancialPlanDTO>> GetFinancialPlanListAsync();
    Task<List<FinancialPlanLazyDTO>> GetFinancialPlanListLazyAsync();
    Task<bool> CreateOrUpdateFinancialPlanAsync(FinancialPlanDTO dto);
    Task<bool> DeleteFinancialPlanAsync(int id);

    // BankAccount
    Task<BankAccountDTO?> GetBankAccountAsync(int id);
    Task<List<BankAccountDTO>> GetBankAccountListAsync();
    Task<bool> CreateOrUpdateBankAccountAsync(BankAccountDTO dto);
    Task<bool> DeleteBankAccountAsync(int id);

    // ExpenseEntry
    Task<ExpenseEntryDTO?> GetExpenseEntryAsync(int id);
    Task<List<ExpenseEntryDTO>> GetExpenseEntryListAsync();
    Task<bool> CreateOrUpdateExpenseEntryAsync(ExpenseEntryDTO dto);
    Task<bool> DeleteExpenseEntryAsync(int id);

    // IncomeEntry
    Task<IncomeEntryDTO?> GetIncomeEntryAsync(int id);
    Task<List<IncomeEntryDTO>> GetIncomeEntryListAsync();
    Task<bool> CreateOrUpdateIncomeEntryAsync(IncomeEntryDTO dto);
    Task<bool> DeleteIncomeEntryAsync(int id);

    // SavingsEntry
    Task<SavingsEntryDTO?> GetSavingsEntryAsync(int id);
    Task<List<SavingsEntryDTO>> GetSavingsEntryListAsync();
    Task<bool> CreateOrUpdateSavingsEntryAsync(SavingsEntryDTO dto);
    Task<bool> DeleteSavingsEntryAsync(int id);

    // SavingsPlan
    Task<SavingsPlanDTO?> GetSavingsPlanAsync(int id);
    Task<List<SavingsPlanDTO>> GetSavingsPlanListAsync();
    Task<bool> CreateOrUpdateSavingsPlanAsync(SavingsPlanDTO dto);
    Task<bool> DeleteSavingsPlanAsync(int id);
}