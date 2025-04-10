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
            .Include(x => x.SavingsPlan)
            .Include(x => x.BankAccounts)
            .Include(x => x.Incomes)
            .Include(x => x.Expenses)
            .SingleOrDefaultAsync(x => x.Id == id);

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

            _db.BankAccounts.AddRange(model.BankAccounts);
            _db.ExpenseEntries.AddRange(model.Expenses);
            _db.IncomeEntries.AddRange(model.Incomes);

            if (model.SavingsPlan is not null)
            {
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

    private async Task<bool> UpdateFinancialPlanAsync(FinancialPlanDTO dto)
    {
        var oldPlan = await _db.FinancialPlans.FindAsync(dto.Id);
        if (oldPlan is null)
        {
            return false;
        }

        var newPlan = dto.ToFinancialPlanModel();

        oldPlan.Name = newPlan.Name;
        oldPlan.Description = newPlan.Description;
        oldPlan.DateModified = DateTime.Now;

        if (oldPlan.BankAccounts.Count > 0)
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
                _db.Entry(oldPlan.SavingsPlan).State = EntityState.Detached;
                oldPlan.SavingsPlan = null;
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

    private async Task<bool> UpdateBankAccountAsync(BankAccountDTO dto)
    {
        var model = dto.ToBankAccountModel();

        var oldAccount = await _db.BankAccounts.FindAsync(dto.Id);
        if (oldAccount is null)
        {
            return false;
        }

        oldAccount.Name = dto.Name;
        oldAccount.Description = dto.Description;

        oldAccount.IBAN = dto.IBAN;
        oldAccount.BIC = dto.BIC;

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
            .SingleOrDefaultAsync(x => x.Id == id);

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

    public async Task<bool> CreateOrUpdateExpenseEntryAsync(ExpenseEntryDTO dto)
    {
        var isNewExpense = await _db.ExpenseEntries.FindAsync(dto.Id) is null;

        return isNewExpense ? await CreateExpenseEntryAsync(dto)
                            : await UpdateExpenseEntryAsync(dto);
    }

    private async Task<bool> CreateExpenseEntryAsync(ExpenseEntryDTO dto)
    {
        var model = dto.ToExpenseEntryModel();

        model.DateCreated = DateTime.Now;
        model.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.ExpenseEntries.Add(model);

            if (model.AssociatedBankAccount is not null)
            {
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
        var oldEntry = await _db.ExpenseEntries.FindAsync(dto.Id);
        if (oldEntry is null)
        {
            return false;
        }

        var newEntry = dto.ToExpenseEntryModel();

        oldEntry.Name = newEntry.Name;
        oldEntry.Description = newEntry.Description;

        oldEntry.Price = newEntry.Price;

        newEntry.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            if (oldEntry.AssociatedBankAccount is not null)
            {
                if (newEntry.AssociatedBankAccount is null)
                {
                    _db.Entry(oldEntry.AssociatedBankAccount).State = EntityState.Detached;
                    oldEntry.AssociatedBankAccount = null;
                }
                else if (newEntry.AssociatedBankAccount.Id != oldEntry.AssociatedBankAccount.Id)
                {
                    _db.Entry(oldEntry.AssociatedBankAccount).State = EntityState.Detached;
                    oldEntry.AssociatedBankAccount = newEntry.AssociatedBankAccount;
                }
            }
            else
            {
                if (newEntry.AssociatedBankAccount is not null)
                {
                    oldEntry.AssociatedBankAccount = newEntry.AssociatedBankAccount;
                }
            }

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
        var entry = await _db.ExpenseEntries.FindAsync(id);
        if (entry is null)
        {
            return false;
        }

        var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.ExpenseEntries.Remove(entry);

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



    #region IncomeEntryModel
    public async Task<IncomeEntryDTO?> GetIncomeEntryAsync(int id)
    {
        return (await _db.IncomeEntries.FindAsync(id))?.ToIncomeEntryDTO();
    }

    public async Task<List<IncomeEntryDTO>> GetIncomeEntryListAsync()
    {
        var entries = await _db.IncomeEntries.ToListAsync();

        var output = new List<IncomeEntryDTO>(entries.Count);
        entries.ForEach(x => output.Add(x.ToIncomeEntryDTO()));

        return output;
    }

    public async Task<bool> CreateOrUpdateIncomeEntryAsync(IncomeEntryDTO dto)
    {
        var isNewEntry = await _db.IncomeEntries.FindAsync(dto.Id) is null;

        return isNewEntry ? await CreateIncomeEntryAsync(dto)
                          : await UpdateIncomeEntryAsync(dto);
    }

    private async Task<bool> CreateIncomeEntryAsync(IncomeEntryDTO dto)
    {
        var model = dto.ToIncomeModel();

        model.DateCreated = DateTime.Now;
        model.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.IncomeEntries.Add(model);

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

    private async Task<bool> UpdateIncomeEntryAsync(IncomeEntryDTO dto)
    {
        var oldEntry = await _db.IncomeEntries.FindAsync(dto.Id);
        if (oldEntry is null)
        {
            return false;
        }

        var newEntry = dto.ToIncomeModel();

        oldEntry.Name = newEntry.Name;
        oldEntry.Description = newEntry.Description;

        oldEntry.TotalAmount = newEntry.TotalAmount;

        newEntry.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
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

    public async Task<bool> DeleteIncomeEntryAsync(int id)
    {
        var entry = await _db.IncomeEntries.FindAsync(id);
        if (entry is null)
        {
            return false;
        }

        var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.IncomeEntries.Remove(entry);

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



    #region SavingsEntry
    public async Task<SavingsEntryDTO?> GetSavingsEntryAsync(int id)
    {
        return (await _db.SavingsEntries.FindAsync(id))?.ToSavingsEntryDTO();
    }

    public async Task<List<SavingsEntryDTO>> GetSavingsEntryListAsync()
    {
        var entries = await _db.SavingsEntries.ToListAsync();

        var output = new List<SavingsEntryDTO>(entries.Count);
        entries.ForEach(x => output.Add(x.ToSavingsEntryDTO()));

        return output;
    }

    public async Task<bool> CreateOrUpdateSavingsEntryAsync(SavingsEntryDTO dto)
    {
        var isNewEntry = await _db.SavingsEntries.FindAsync(dto.Id) is null;

        return isNewEntry ? await CreateSavingsEntryAsync(dto)
                          : await UpdateSavingsEntryAsync(dto);
    }

    private async Task<bool> CreateSavingsEntryAsync(SavingsEntryDTO dto)
    {
        var model = dto.ToSavingsEntryModel();

        model.DateCreated = DateTime.Now;
        model.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.SavingsEntries.Add(model);

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

    private async Task<bool> UpdateSavingsEntryAsync(SavingsEntryDTO dto)
    {
        var oldEntry = await _db.SavingsEntries.FindAsync(dto.Id);
        if (oldEntry is null)
        {
            return false;
        }

        var newEntry = dto.ToSavingsEntryModel();

        oldEntry.Name = newEntry.Name;
        oldEntry.Description = newEntry.Description;

        oldEntry.Price = newEntry.Price;

        newEntry.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
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

    public async Task<bool> DeleteSavingsEntryAsync(int id)
    {
        var entry = await _db.SavingsEntries.FindAsync(id);
        if (entry is null)
        {
            return false;
        }

        var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.SavingsEntries.Remove(entry);

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



    #region SavingsPlan
    public async Task<SavingsPlanDTO?> GetSavingsPlanAsync(int id)
    {
        var plan = await _db.SavingsPlans
            .Include(s => s.SavingsEntries)
            .SingleOrDefaultAsync(x => x.Id == id);

        return plan?.ToSavingsPlanDTO();
    }

    public async Task<List<SavingsPlanDTO>> GetSavingsPlanListAsync()
    {
        var plans = await _db.SavingsPlans
            .Include(s => s.SavingsEntries)
            .ToListAsync();

        var output = new List<SavingsPlanDTO>(plans.Count);
        plans.ForEach(x => output.Add(x.ToSavingsPlanDTO()));

        return output;
    }

    public async Task<bool> CreateOrUpdateSavingsPlanAsync(SavingsPlanDTO dto)
    {
        var isNewPlan = await _db.SavingsPlans.FindAsync(dto.Id) is null;

        return isNewPlan ? await CreateSavingsPlanAsync(dto)
                         : await UpdateSavingsPlanAsync(dto);
    }

    private async Task<bool> CreateSavingsPlanAsync(SavingsPlanDTO dto)
    {
        var model = dto.ToSavingsPlanModel();

        model.DateCreated = DateTime.Now;
        model.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.SavingsPlans.Add(model);

            _db.SavingsEntries.AddRange(model.SavingsEntries);

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

    private async Task<bool> UpdateSavingsPlanAsync(SavingsPlanDTO dto)
    {
        var oldPlan = await _db.SavingsPlans.FindAsync(dto.Id);
        if (oldPlan is null)
        {
            return false;
        }

        var newPlan = dto.ToSavingsPlanModel();

        oldPlan.Name = newPlan.Name;
        oldPlan.Description = newPlan.Description;
        oldPlan.DateModified = DateTime.Now;

        if (oldPlan.SavingsEntries.Count > 0)
        {
            if (newPlan.SavingsEntries.Count > 0)
            {
                var entriesToDelete = oldPlan.SavingsEntries.Except(newPlan.SavingsEntries)
                                                          .ToList();
                entriesToDelete.ForEach(x => oldPlan.SavingsEntries.Remove(x));

                var entriesToAdd = newPlan.SavingsEntries.Except(oldPlan.SavingsEntries)
                                                       .ToList();
                oldPlan.SavingsEntries.AddRange(entriesToAdd);
            }
            else
            {
                oldPlan.SavingsEntries.Clear();
            }
        }
        else
        {
            if (newPlan.SavingsEntries.Count > 0)
            {
                oldPlan.SavingsEntries.AddRange(newPlan.SavingsEntries);
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

    public async Task<bool> DeleteSavingsPlanAsync(int id)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            var plan = await _db.SavingsPlans
                .Include(s => s.SavingsEntries)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (plan is null)
            {
                await transaction.RollbackAsync();
                return false;
            }

            _db.SavingsEntries.RemoveRange(plan.SavingsEntries);

            _db.SavingsPlans.Remove(plan);

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
}