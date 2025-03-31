using HFTLibrary.DBContexts;
using HFTLibrary.DTOs;
using HFTLibrary.Logic;

using Microsoft.EntityFrameworkCore;

namespace HFTLibrary.Data;

public class EFCoreData : IEFCoreData
{
    private readonly EFCoreContext _db;

    public EFCoreData(EFCoreContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    #region FinancialPlan
    public async Task<FinancialPlanDTO?> GetFinancialPlanAsync(int id)
    {
        var plan = await _db.FinancialPlans
            .Include(s => s.SavingsPlan)
            .Include(b => b.BankAccounts)
            .Include(i => i.Incomes)
            .Include(e => e.Expenses)
            .Where(p => p.Id == id)
            .SingleOrDefaultAsync();

        return plan?.ToFinancialPlanDTO();
    }

    public async Task<FinancialPlanLazyDTO?> GetFinancialPlanLazyAsync(int id)
    {
        return await _db.FinancialPlans
            .Where(p => p.Id == id)
            .Select(p => new FinancialPlanLazyDTO
            {
                Id = p.Id,
                Name = p.Name
            })
            .SingleOrDefaultAsync();
    }

    public async Task<List<FinancialPlanDTO>> GetFinancialPlanListAsync()
    {
        var plans = await _db.FinancialPlans
            .Include(s => s.SavingsPlan)
            .Include(b => b.BankAccounts)
            .Include(i => i.Incomes)
            .Include(e => e.Expenses)
            .ToListAsync();

        var output = new List<FinancialPlanDTO>(plans.Count);
        plans.ForEach(x => output.Add(x.ToFinancialPlanDTO()));

        return output;
    }

    public async Task<List<FinancialPlanLazyDTO>> GetFinancialPlanListLazyAsync()
    {
        List<FinancialPlanLazyDTO> output = await _db.FinancialPlans
            .Select(p => new FinancialPlanLazyDTO
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync();

        return output;
    }

    public async Task<bool> CreateOrUpdateFinancialPlanAsync(FinancialPlanDTO dto)
    {
        var isNewPlan = await _db.FinancialPlans.FindAsync(dto.Id) is null;

        return isNewPlan ? await CreateFinancialPlanAsync(dto)
                         : await UpdateFinancialPlanAsync(dto);
    }

    private async Task<bool> CreateFinancialPlanAsync(FinancialPlanDTO dto)
    {
        var model = dto.ToFinancialPlanModel();
        model.DateCreated = DateTime.Now;
        model.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.FinancialPlans.Add(model);

            if (model.BankAccounts is not null)
            {
                model.BankAccounts.ForEach(x =>
                {
                    x.DateCreated = DateTime.Now;
                    x.DateModified = DateTime.Now;
                });

                _db.BankAccounts.AddRange(model.BankAccounts);
            }

            if (model.Expenses is not null)
            {
                model.Expenses.ForEach(x =>
                {
                    x.DateCreated = DateTime.Now;
                    x.DateModified = DateTime.Now;
                });

                _db.ExpenseEntries.AddRange(model.Expenses);
            }

            if (model.Incomes is not null)
            {
                model.Incomes.ForEach(x =>
                {
                    x.DateCreated = DateTime.Now;
                    x.DateModified = DateTime.Now;
                });

                _db.IncomeEntries.AddRange(model.Incomes);
            }

            if (model.SavingsPlan is not null)
            {
                model.SavingsPlan.DateCreated = DateTime.Now;
                model.SavingsPlan.DateModified = DateTime.Now;

                _db.SavingsPlans.Add(model.SavingsPlan);
            }

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> UpdateFinancialPlanAsync(FinancialPlanDTO dto)
    {
        var oldPlan = (await GetFinancialPlanAsync(dto.Id))?.ToFinancialPlanModel();
        if (oldPlan is null)
        {
            throw new InvalidOperationException($"Could not load financial plan with ID {dto.Id}");
        }

        var newPlan = dto.ToFinancialPlanModel();
        oldPlan.DateModified = DateTime.Now;

        if (oldPlan.BankAccounts!.Count > 0)
        {
            if (newPlan.BankAccounts.Count > 0)
            {
                var entriesToDelete = oldPlan.BankAccounts.Except(newPlan.BankAccounts)
                                                          .ToList();
                entriesToDelete.ForEach(x => oldPlan.BankAccounts.Remove(x));

                var entriesToAdd = newPlan.BankAccounts.Except(oldPlan.BankAccounts)
                                                       .ToList();
                oldPlan.BankAccounts.AddRange(entriesToAdd);
            }
            else
            {
                oldPlan.BankAccounts.Clear();
            }
        }
        else
        {
            if (newPlan.BankAccounts.Count > 0)
            {
                oldPlan.BankAccounts.AddRange(newPlan.BankAccounts);
            }
        }

        if (oldPlan.Expenses!.Count > 0)
        {
            if (newPlan.Expenses.Count > 0)
            {
                var entriesToDelete = oldPlan.Expenses.Except(newPlan.Expenses)
                                                          .ToList();
                entriesToDelete.ForEach(x => oldPlan.Expenses.Remove(x));

                var entriesToAdd = newPlan.Expenses.Except(oldPlan.Expenses)
                                                       .ToList();
                oldPlan.Expenses.AddRange(entriesToAdd);
            }
            else
            {
                oldPlan.Expenses.Clear();
            }
        }
        else
        {
            if (newPlan.Expenses.Count > 0)
            {
                oldPlan.Expenses.AddRange(newPlan.Expenses);
            }
        }

        if (oldPlan.Incomes!.Count > 0)
        {
            if (newPlan.Incomes.Count > 0)
            {
                var entriesToDelete = oldPlan.Incomes.Except(newPlan.Incomes)
                                                          .ToList();
                entriesToDelete.ForEach(x => oldPlan.Incomes.Remove(x));

                var entriesToAdd = newPlan.Incomes.Except(oldPlan.Incomes)
                                                       .ToList();
                oldPlan.Incomes.AddRange(entriesToAdd);
            }
            else
            {
                oldPlan.Incomes.Clear();
            }
        }
        else
        {
            if (newPlan.Incomes.Count > 0)
            {
                oldPlan.Incomes.AddRange(newPlan.Incomes);
            }
        }

        if (oldPlan.SavingsPlan is not null)
        {
            if (newPlan.SavingsPlan is null)
            {
                oldPlan.SavingsPlan = null;
                _db.Entry(oldPlan.SavingsPlan!).State = EntityState.Detached;
            }
            else
            {
                oldPlan.SavingsPlan = newPlan.SavingsPlan;
            }
        }
        else
        {
            if (newPlan.SavingsPlan is not null)
            {
                oldPlan.SavingsPlan = newPlan.SavingsPlan;
            }
        }

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.Entry(oldPlan).State = EntityState.Modified;

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> DeleteFinancialPlanAsync(int id)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            var plan = await _db.FinancialPlans
                .Include(ba => ba.BankAccounts)
                .Include(e => e.Expenses)
                .Include(i => i.Incomes)
                .Include(sp => sp.SavingsPlan)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (plan is null)
            {
                await transaction.RollbackAsync();
                return false;
            }

            _db.BankAccounts.RemoveRange(plan.BankAccounts);
            _db.ExpenseEntries.RemoveRange(plan.Expenses);
            _db.IncomeEntries.RemoveRange(plan.Incomes);

            _db.FinancialPlans.Remove(plan);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
    #endregion



    #region BankAccount
    public async Task<BankAccountDTO?> GetBankAccountAsync(int id)
    {
        var account = await _db.BankAccounts.FindAsync(id);
        return account?.ToBankAccountDTO();
    }

    public async Task<List<BankAccountDTO>> GetBankAccountListAsync()
    {
        var accounts = await _db.BankAccounts.ToListAsync();

        var output = new List<BankAccountDTO>(accounts.Count);
        accounts.ForEach(x => output.Add(x.ToBankAccountDTO()));

        return output;
    }

    public async Task<bool> CreateOrUpdateBankAccountAsync(BankAccountDTO dto)
    {
        var isNewAccount = await _db.BankAccounts.FindAsync(dto.Id) is null;

        return isNewAccount ? await CreateBankAccountAsync(dto)
                            : await UpdateBankAccountAsync(dto);
    }

    private async Task<bool> CreateBankAccountAsync(BankAccountDTO dto)
    {
        var model = dto.ToBankAccountModel();
        model.DateCreated = DateTime.Now;
        model.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.BankAccounts.Add(model);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    private async Task<bool> UpdateBankAccountAsync(BankAccountDTO account)
    {
        // TODO: check implementation of UpdateBankAccountAsync()
        var model = account.ToBankAccountModel();
        var oldAccount = await _db.BankAccounts.FindAsync(account.Id);
        if (oldAccount is null)
        {
            return false;
        }

        oldAccount.MapFromBankAccountModel(model);
        oldAccount.DateModified = DateTime.Now;

        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteBankAccountAsync(int id)
    {
        var account = await _db.BankAccounts.FindAsync(id);
        if (account is null)
        {
            return false;
        }

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.BankAccounts.Remove(account);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
    #endregion



    #region ExpenseEntry
    public async Task<ExpenseEntryDTO?> GetExpenseEntryAsync(int id)
    {
        var output = await _db.ExpenseEntries
            .Include(x => x.AssociatedBankAccount)
            .SingleOrDefaultAsync();

        return output?.ToExpenseEntryDTO();
    }

    public async Task<List<ExpenseEntryDTO>> GetExpenseEntryListAsync()
    {
        var entries = await _db.ExpenseEntries
            .Include(x => x.AssociatedBankAccount)
            .ToListAsync();

        var output = new List<ExpenseEntryDTO>(entries.Count);
        entries.ForEach(x => output.Add(x.ToExpenseEntryDTO()));

        return output;
    }

    public Task<bool> CreateOrUpdateExpenseEntryAsync(ExpenseEntryDTO dto)
    {
        // TODO: implement CreateOrUpdateExpenseEntryAsync()
        throw new NotImplementedException();
    }

    private async Task<bool> CreateExpenseEntryAsync(ExpenseEntryDTO entry)
    {
        var model = entry.ToExpenseEntryModel();
        model.DateCreated = DateTime.Now;
        model.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.ExpenseEntries.Add(model);

            if (model.AssociatedBankAccount is not null)
            {
                model.AssociatedBankAccount.DateCreated = DateTime.Now;
                model.AssociatedBankAccount.DateModified = DateTime.Now;

                _db.BankAccounts.Add(model.AssociatedBankAccount);
            }

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    private async Task<bool> UpdateExpenseEntryAsync(ExpenseEntryDTO dto)
    {
        var oldEntry = (await GetExpenseEntryAsync(dto.Id))?.ToExpenseEntryModel();
        if (oldEntry is null)
        {
            throw new InvalidOperationException($"Could not load expense entry with ID {dto.Id}");
        }

        var newEntry = dto.ToExpenseEntryModel();
        newEntry.DateModified = DateTime.Now;

        oldEntry.MapFromExpenseEntryModel(newEntry);

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.Entry(oldEntry.Name).State = EntityState.Modified;
            _db.Entry(oldEntry.Description).State = EntityState.Modified;

            _db.Entry(oldEntry.Price).State = EntityState.Modified;

            if (oldEntry.AssociatedBankAccount is not null)
            {
                _db.Entry(oldEntry.AssociatedBankAccount).State = EntityState.Modified;
            }

            _db.Entry(oldEntry.DateModified).State = EntityState.Modified;

            _db.Entry(oldEntry).State = EntityState.Modified;

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> DeleteExpenseEntryAsync(int id)
    {
        // TODO: implement DeleteExpenseEntryAsync()
        throw new NotImplementedException();
    }
    #endregion



    #region IncomeEntryModel
    public Task<IncomeEntryDTO?> GetIncomeEntryAsync(int id)
    {
        // TODO: implement GetIncomeEntryAsync()
        throw new NotImplementedException();
    }

    public Task<List<IncomeEntryDTO>> GetIncomeEntryListAsync()
    {
        // TODO: implement GetIncomeEntryListAsync()
        throw new NotImplementedException();
    }

    public Task<bool> CreateOrUpdateIncomeEntryAsync(IncomeEntryDTO dto)
    {
        // TODO: implement CreateOrUpdateIncomeEntryAsync()
        throw new NotImplementedException();
    }

    private Task<bool> CreateIncomeEntryAsync(IncomeEntryDTO dto)
    {
        // TODO: implement CreateIncomeEntryAsync()
        throw new NotImplementedException();
    }

    private Task<bool> UpdateIncomeEntryAsync(IncomeEntryDTO dto)
    {
        // TODO: implement UpdateIncomeEntryAsync()
        throw new NotImplementedException();
    }

    public Task<bool> DeleteIncomeEntryAsync(int id)
    {
        // TODO: implement DeleteIncomeEntryAsync()
        throw new NotImplementedException();
    }
    #endregion



    #region SavingsEntry
    public Task<SavingsEntryDTO?> GetSavingsEntryAsync(int id)
    {
        // TODO: implement GetSavingsEntryAsync()
        throw new NotImplementedException();
    }

    public Task<List<SavingsEntryDTO>> GetSavingsEntryListAsync()
    {
        // TODO: implement GetSavingsEntryAsync()
        throw new NotImplementedException();
    }

    public Task<bool> CreateOrUpdateSavingsEntryAsync(SavingsEntryDTO dto)
    {
        // TODO: implement CreateOrUpdateSavingsEntryAsync()
        throw new NotImplementedException();
    }

    private Task<bool> CreateSavingsEntryAsync(SavingsEntryDTO dto)
    {
        // TODO: implement CreateSavingsEntryAsync()
        throw new NotImplementedException();
    }

    private Task<bool> UpdateSavingsEntryAsync(SavingsEntryDTO dto)
    {
        // TODO: implement UpdateSavingsEntryAsync()
        throw new NotImplementedException();
    }

    public Task<bool> DeleteSavingsEntryAsync(int id)
    {
        // TODO: implement DeleteSavingsEntryAsync()
        throw new NotImplementedException();
    }
    #endregion



    #region SavingsPlan
    public Task<SavingsPlanDTO?> GetSavingsPlanAsync(int id)
    {
        // TODO: implement GetSavingsPlanAsync()
        throw new NotImplementedException();
    }

    public Task<List<SavingsPlanDTO>> GetSavingsPlanListAsync()
    {
        // TODO: implement GetSavingsPlanListAsync()
        throw new NotImplementedException();
    }

    public Task<bool> CreateOrUpdateSavingsPlanAsync(SavingsPlanDTO dto)
    {
        // TODO: implement CreateOrUpdateSavingsPlanAsync()
        throw new NotImplementedException();
    }

    private Task<bool> CreateSavingsPlanAsync(SavingsPlanDTO dto)
    {
        // TODO: implement CreateSavingsPlanAsync()
        throw new NotImplementedException();
    }

    private Task<bool> UpdateSavingsPlanAsync(SavingsPlanDTO dto)
    {
        // TODO: implement UpdateSavingsPlanAsync()
        throw new NotImplementedException();
    }

    public Task<bool> DeleteSavingsPlanAsync(int id)
    {
        // TODO: implement DeleteSavingsPlanAsync()
        throw new NotImplementedException();
    }
    #endregion
}