using HFTLibrary.Models;
using HFTLibrary.DTOs;

namespace HFTLibrary.Data;

public interface IEFCoreData
{
    // FinancialPlan
    Task<FinancialPlanDTO?> GetFinancialPlanAsync(int id);
    Task<FinancialPlanDTO?> GetFinancialPlanLazyAsync(int id);
    Task<List<FinancialPlanDTO>> GetFinancialPlanListAsync();
    Task<List<(int Id, string Name)>> GetFinancialPlanListLazyAsync();
    Task CreateFinancialPlanAsync(FinancialPlanDTO plan);
    Task UpdateFinancialPlanAsync(FinancialPlanDTO plan);
    Task DeleteFinancialPlanAsync(int id);

    // BankAccount
    Task<BankAccountDTO?> GetBankAccountAsync(int id);
    Task<List<BankAccountDTO>> GetBankAccountListAsync();
    Task CreateBankAccountAsync(BankAccountDTO account);
    Task UpdateBankAccountAsync(BankAccountDTO account);
    Task DeleteBankAccountAsync(int id);

    // ExpenseEntry
    Task<ExpenseEntryDTO?> GetExpenseEntryAsync(int id);
    Task<List<ExpenseEntryDTO>> GetExpenseEntryListAsync();
    Task CreateExpenseEntryAsync(ExpenseEntryDTO entry);
    Task UpdateExpenseEntryAsync(ExpenseEntryDTO entry);
    Task DeleteExpenseEntryAsync(int id);

    // IncomeEntry

    // SavingsEntry

    // SavingsPlan
}