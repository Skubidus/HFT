using HFTLibrary.DBContexts;
using HFTLibrary.DTOs;
using HFTLibrary.Logic;
using HFTLibrary.Models;

using Microsoft.EntityFrameworkCore;

using System.Xml;

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
    public async Task<FinancialPlanDTO?> GetFinancialPlanAsync(int id)
    {
        return await _db.FinancialPlans
            .Include(p => p.SavingsPlan)
            .Include(p => p.BankAccounts)
            .Include(p => p.Incomes)
            .Include(p => p.Expenses)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<FinancialPlanDTO?> GetFinancialPlanLazyAsync(int id)
    {
        return await _db.FinancialPlans
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<FinancialPlanDTO>> GetFinancialPlanListAsync()
    {
        return await _db.FinancialPlans
            .Include(p => p.SavingsPlan)
            .Include(p => p.BankAccounts)
            .Include(p => p.Incomes)
            .Include(p => p.Expenses)
            .ToListAsync();
    }

    public async Task<List<(int Id, string Name)>> GetFinancialPlanListLazyAsync()
    {
        var query = _db.FinancialPlans.Select(x => new { x.Id, x.Name });
        var resultList = await query.ToListAsync();
        return resultList.Select(x => (x.Id, x.Name)).ToList();
    }

    public async Task CreateFinancialPlanAsync(FinancialPlanDTO plan)
    {
        _ = _db.FinancialPlans.Add(plan);
        _ = await _db.SaveChangesAsync();
    }

    public async Task UpdateFinancialPlanAsync(FinancialPlanDTO plan)
    {
        var originalPlan = await GetFinancialPlanAsync(plan.Id);
        originalPlan = plan;
        _ = await _db.SaveChangesAsync();
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
    public async Task<BankAccountDTO?> GetBankAccountAsync(int id)
    {
        // TODO: implement GetBankAccountAsync()
        throw new NotImplementedException();
    }

    public async Task<List<BankAccountDTO>> GetBankAccountListAsync()
    {
        // TODO: implement GetBankAccountListAsync()
        throw new NotImplementedException();
    }

    public async Task CreateBankAccountAsync(BankAccountDTO account)
    {
        // TODO: implement CreateBankAccountAsync()
        throw new NotImplementedException();
    }

    public async Task UpdateBankAccountAsync(BankAccountDTO account)
    {
        // TODO: implement UpdateBankAccountAsync()
        throw new NotImplementedException();
    }

    public async Task DeleteBankAccountAsync(int id)
    {
        // TODO: implement DeleteBankAccountAsync()
        throw new NotImplementedException();
    }
    #endregion



    #region ExpenseEntryModel
    public async Task<ExpenseEntryDTO?> GetExpenseEntryAsync(int id)
    {
        var model = await GetExpenseEntryModelAsync(id);

        if (model is null)
        {
            return null;
        }

        var output = new ExpenseEntryDTO
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            AssociatedBankAccountId = model.AssociatedBankAccount?.Id,
            DateCreated = model.DateCreated,
            DateModified = model.DateModified
        };

        return output;
    }

    private async Task<ExpenseEntryModel?> GetExpenseEntryModelAsync(int id)
    {
        return await _db.ExpenseEntries
            .Include(e => e.AssociatedBankAccount)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<ExpenseEntryDTO>> GetExpenseEntryListAsync()
    {
        // TODO: implement GetExpenseEntryListAsync()
        throw new NotImplementedException();
    }

    public async Task CreateExpenseEntryAsync(ExpenseEntryDTO entry)
    {
        // TODO: implement CreateExpenseEntryAsync()
        throw new NotImplementedException();
    }

    public async Task UpdateExpenseEntryAsync(ExpenseEntryDTO entry)
    {
        // TODO: UpdateExpenseEntryAsync() needs testing

        var originalEntry = await GetExpenseEntryAsync(entry.Id)
            ?? throw new InvalidOperationException($"{nameof(ExpenseEntryModel)} with ID {entry.Id} not found in database.");

        originalEntry.Name = entry.Name;
        originalEntry.Description = entry.Description;
        originalEntry.Price = entry.Price;
        if (entry.AssociatedBankAccountId.HasValue)
        {
            originalEntry.AssociatedBankAccount = GetBankAccount(entry.AssociatedBankAccountId.Value);
        }
        originalEntry.DateModified = DateTime.Now;

        _ = await _db.SaveChangesAsync();
    }

    public async Task DeleteExpenseEntryAsync(int id)
    {
        // TODO: implement DeleteExpenseEntryAsync()
        throw new NotImplementedException();
    }
    #endregion



    #region IncomeEntryModel
    #endregion



    #region SavingsPlanModel
    #endregion



    #region SavingsEntryModel
    #endregion
}