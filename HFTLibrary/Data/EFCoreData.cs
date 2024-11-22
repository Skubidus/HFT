using HFTLibrary.DBContexts;
using HFTLibrary.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Runtime.CompilerServices;

namespace HFTLibrary.Data;

public class EFCoreData : IEFCoreData
{
    private readonly EFCoreContext _db;
    //private readonly ILogger _logger;

    public EFCoreData(EFCoreContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        //_logger = logger;
    }

    #region FinancialPlanModel
    public async Task<List<(int Id, string Name)>> GetShortFinancialPlanListAsync()
    {
        var query = _db.FinancialPlans.Select(x => new { x.Id, x.Name });
        var resultList = await query.ToListAsync();
        return resultList.Select(x => (x.Id, x.Name)).ToList();
    }

    public async Task<List<FinancialPlanModel>> GetFullFinancialPlanListAsync()
    {
        return await _db.FinancialPlans
            .Include(p => p.SavingsPlan)
            .Include(p => p.BankAccounts)
            .Include(p => p.Incomes)
            .Include(p => p.Expenses)
            .ToListAsync();
    }

    public async Task<FinancialPlanModel?> GetFinancialPlanAsync(int id)
    {
        return await _db.FinancialPlans
            .Include(p => p.SavingsPlan)
            .Include(p => p.BankAccounts)
            .Include(p => p.Incomes)
            .Include(p => p.Expenses)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateFinancialPlanAsync(FinancialPlanModel plan)
    {
        _ = _db.FinancialPlans.Add(plan);
        _ = await _db.SaveChangesAsync();
    }

    public void UpdateFinancialPlan(FinancialPlanModel plan)
    {
        // TODO: implement UpdateFinancialPlan()
        throw new NotImplementedException();
    }

    public async Task DeleteFinancialPlanAsync(int id)
    {
        var plan = _db.FinancialPlans
            .Include(p => p.SavingsPlan)
            .Include(p => p.BankAccounts)
            .Include(p => p.Incomes)
            .Include(p => p.Expenses)
            .Single(x => x.Id == id);

        _ = _db.FinancialPlans.Remove(plan);
        _ = await _db.SaveChangesAsync();
    }
    #endregion

    #region BankAccountModel
    public List<BankAccountModel> GetAllBankAccounts()
    {
        // TODO: implement GetAllBankAccounts()
        throw new NotImplementedException();
    }

    public BankAccountModel? GetBankAccount(int id)
    {
        // TODO: implement GetBankAccount()
        throw new NotImplementedException();
    }

    public void CreateBankAccount(BankAccountModel account)
    {
        // TODO: implement CreateBankAccount()
        throw new NotImplementedException();
    }

    public void UpdateBankAccount(BankAccountModel account)
    {
        // TODO: implement UpdateBankAcocunt()
        throw new NotImplementedException();
    }

    public void DeleteBankAccount(int id)
    {
        // TODO: implement DeleteBankAccount()
        throw new NotImplementedException();
    }
    #endregion

    #region ExpenseEntryModel
    public List<ExpenseEntryModel> GetAllExpenseEntries()
    {
        // TODO: implement GetAllExpenseEntries()
        throw new NotImplementedException();
    }

    public ExpenseEntryModel? GetExpenseEntry(int id)
    {
        // TODO: implement GetExpenseEntry()
        throw new NotImplementedException();
    }

    public void CreateExpenseEntry(ExpenseEntryModel entry)
    {
        // TODO: implement CreateExpenseEntry()
        throw new NotImplementedException();
    }

    public void UpdateExpenseEntry(ExpenseEntryModel entry)
    {
        // TODO: implement UpdateExpenseEntry()
        throw new NotImplementedException();
    }

    public void DeleteExpenseEntry(int id)
    {
        // TODO: implement DeleteExpenseEntry()
        throw new NotImplementedException();
    }
    #endregion

    #region IncomeEntryModel
    public List<IncomeEntryModel> GetAllIncomeEntries()
    {
        // TODO: implement GetAllIncomeEntries()
        throw new NotImplementedException();
    }

    public IncomeEntryModel? GetIncomeEntry(int id)
    {
        // TODO: implement GetIncomeEntry()
        throw new NotImplementedException();
    }

    public void CreateIncomeEntry(IncomeEntryModel entry)
    {
        // TODO: implement CreateIncomeEntry()
        throw new NotImplementedException();
    }

    public void UpdateIncomeEntry(IncomeEntryModel entry)
    {
        // TODO: implement UpdateIncomeEntry()
        throw new NotImplementedException();
    }

    public void DeleteIncomeEntry(int id)
    {
        // TODO: implement DeleteIncomeEntry()
        throw new NotImplementedException();
    }
    #endregion

    #region SavingsPlanModel
    public SavingsPlanModel? GetSavingsPlan(int id)
    {
        // TODO: implement GetSavingsPlan()
        throw new NotImplementedException();
    }

    public void CreateSavingsPlan(SavingsPlanModel plan)
    {
        // TODO: implement CreateSavingsPlan()
        throw new NotImplementedException();
    }

    public void UpdateSavingsPlan(SavingsPlanModel plan)
    {
        // TODO: implement UpdateSavingsPlan()
        throw new NotImplementedException();
    }

    public void DeleteSavingsPlan(int id)
    {
        // TODO: implement DeleteSavingsPlan()
        throw new NotImplementedException();
    }
    #endregion

    #region SavingsEntryModel
    public List<SavingsEntryModel> GetAllSavingsEntries()
    {
        // TODO: implement GetAllSavingsEntries()
        throw new NotImplementedException();
    }

    public SavingsEntryModel? GetSavingsEntry(int id)
    {
        // TODO: implement GetSavingsEntry()
        throw new NotImplementedException();
    }

    public void CreateSavingsEntry(SavingsEntryModel entry)
    {
        // TODO: implement CreateSavingsEntry()
        throw new NotImplementedException();
    }

    public void UpdateSavingsEntry(SavingsEntryModel entry)
    {
        // TODO: implement UpdateSavingsEntry()
        throw new NotImplementedException();
    }

    public void DeleteSavingsEntry(int id)
    {
        // TODO: implement DeleteSavingsEntry()
        throw new NotImplementedException();
    }
    #endregion
}