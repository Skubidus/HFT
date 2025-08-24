using HFTLibrary.DBContexts;
using HFTLibrary.DTOs;
using HFTLibrary.Logic;

using Microsoft.EntityFrameworkCore;

using System.Security.Principal;

namespace HFTLibrary.Data;

/// <summary>
/// Provides data access operations for financial plans using Entity Framework Core.
/// </summary>
public class EFCoreData : IEFCoreData
{
    private readonly EFCoreContext _db;

    /// <summary>
    /// Initializes a new instance of the <see cref="EFCoreData"/> class.
    /// </summary>
    /// <param name="db">The Entity Framework Core database context.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="db"/> is null.</exception>
    public EFCoreData(EFCoreContext db)
    {
        ArgumentNullException.ThrowIfNull(db);

        _db = db;
    }

    #region FinancialPlan

    /// <summary>
    /// Retrieves a financial plan by its ID, including related savings plan, bank accounts, incomes, and expenses.
    /// </summary>
    /// <param name="id">The ID of the financial plan to retrieve.</param>
    /// <returns>A <see cref="FinancialPlanDTO"/> representing the financial plan, or null if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    public async Task<FinancialPlanDTO?> GetFinancialPlanAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        var plan = await _db.FinancialPlans
            .Include(x => x.SavingsPlan)
            .Include(x => x.BankAccounts)
            .Include(x => x.Incomes)
            .Include(x => x.Expenses)
            .SingleOrDefaultAsync(x => x.Id == id);

        return plan?.ToFinancialPlanDTO();
    }

    /// <summary>
    /// Retrieves a lightweight financial plan by its ID, including only the ID and name.
    /// </summary>
    /// <param name="id">The ID of the financial plan to retrieve.</param>
    /// <returns>A <see cref="FinancialPlanLazyDTO"/> with the ID and name, or null if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    public async Task<FinancialPlanLazyDTO?> GetFinancialPlanLazyAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        return await _db.FinancialPlans
            .Where(p => p.Id == id)
            .Select(p => new FinancialPlanLazyDTO
            {
                Id = p.Id,
                Name = p.Name
            })
            .SingleOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves a list of all financial plans, including related savings plans, bank accounts, incomes, and expenses.
    /// </summary>
    /// <returns>A list of <see cref="FinancialPlanDTO"/> objects representing all financial plans.</returns>
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

    /// <summary>
    /// Retrieves a list of lightweight financial plans, including only IDs and names.
    /// </summary>
    /// <returns>A list of <see cref="FinancialPlanLazyDTO"/> objects.</returns>
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

    /// <summary>
    /// Creates or updates a financial plan based on the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="FinancialPlanDTO"/> containing the financial plan data.</param>
    /// <returns>True if the operation is successful, false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    public async Task<bool> CreateOrUpdateFinancialPlanAsync(FinancialPlanDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var isNewPlan = await _db.FinancialPlans.FindAsync(dto.Id) is null;

        return isNewPlan ? await CreateFinancialPlanAsync(dto)
                         : await UpdateFinancialPlanAsync(dto);
    }

    /// <summary>
    /// Creates a new financial plan from the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="FinancialPlanDTO"/> containing the financial plan data.</param>
    /// <returns>True if the creation is successful, false if an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    private async Task<bool> CreateFinancialPlanAsync(FinancialPlanDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

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
            throw;
        }
    }

    /// <summary>
    /// Updates an existing financial plan with the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="FinancialPlanDTO"/> containing the updated financial plan data.</param>
    /// <returns>True if the update is successful, false if the plan is not found or an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    private async Task<bool> UpdateFinancialPlanAsync(FinancialPlanDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var oldPlan = await _db.FinancialPlans.FindAsync(dto.Id);
        if (oldPlan is null)
        {
            return false;
        }

        var newPlan = dto.ToFinancialPlanModel();

        oldPlan.Name = newPlan.Name;
        oldPlan.Description = newPlan.Description;
        oldPlan.DateModified = DateTime.Now;

        //if (oldPlan.BankAccounts.Count > 0)
        //{
        //    if (newPlan.BankAccounts.Count > 0)
        //    {
        //        var entriesToDelete = oldPlan.BankAccounts.Except(newPlan.BankAccounts).ToList();
        //        //entriesToDelete.ForEach(x => oldPlan.BankAccounts.Remove(x));

        //        entriesToDelete.ForEach(x => oldPlan.BankAccounts.Remove(x));
        //        entriesToDelete.ForEach(async x => await DeleteBankAccountAsync(x.Id));
        //        //_db.BankAccounts.RemoveRange(entriesToDelete);

        //        var entriesToAdd = newPlan.BankAccounts.Except(oldPlan.BankAccounts).ToList();
        //        oldPlan.BankAccounts.AddRange(entriesToAdd);
        //    }
        //    else
        //    {
        //        oldPlan.BankAccounts.Clear();
        //    }
        //}
        //else
        //{
        //    if (newPlan.BankAccounts.Count > 0)
        //    {
        //        oldPlan.BankAccounts.AddRange(newPlan.BankAccounts);
        //    }
        //}

        //if (oldPlan.Expenses!.Count > 0)
        //{
        //    if (newPlan.Expenses.Count > 0)
        //    {
        //        var entriesToDelete = oldPlan.Expenses.Except(newPlan.Expenses).ToList();
        //        entriesToDelete.ForEach(x => oldPlan.Expenses.Remove(x));

        //        var entriesToAdd = newPlan.Expenses.Except(oldPlan.Expenses).ToList();
        //        oldPlan.Expenses.AddRange(entriesToAdd);
        //    }
        //    else
        //    {
        //        oldPlan.Expenses.Clear();
        //    }
        //}
        //else
        //{
        //    if (newPlan.Expenses.Count > 0)
        //    {
        //        oldPlan.Expenses.AddRange(newPlan.Expenses);
        //    }
        //}

        //if (oldPlan.Incomes!.Count > 0)
        //{
        //    if (newPlan.Incomes.Count > 0)
        //    {
        //        var entriesToDelete = oldPlan.Incomes.Except(newPlan.Incomes).ToList();
        //        entriesToDelete.ForEach(x => oldPlan.Incomes.Remove(x));

        //        var entriesToAdd = newPlan.Incomes.Except(oldPlan.Incomes).ToList();
        //        oldPlan.Incomes.AddRange(entriesToAdd);
        //    }
        //    else
        //    {
        //        oldPlan.Incomes.Clear();
        //    }
        //}
        //else
        //{
        //    if (newPlan.Incomes.Count > 0)
        //    {
        //        oldPlan.Incomes.AddRange(newPlan.Incomes);
        //    }
        //}

        //if (oldPlan.SavingsPlan is not null)
        //{
        //    if (newPlan.SavingsPlan is null)
        //    {
        //        _db.Entry(oldPlan.SavingsPlan).State = EntityState.Detached;
        //        oldPlan.SavingsPlan = null;
        //    }
        //    else
        //    {
        //        oldPlan.SavingsPlan = newPlan.SavingsPlan;
        //    }
        //}
        //else
        //{
        //    if (newPlan.SavingsPlan is not null)
        //    {
        //        oldPlan.SavingsPlan = newPlan.SavingsPlan;
        //    }
        //}

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
            throw;
        }
    }

    /// <summary>
    /// Deletes a financial plan by its ID, including related bank accounts, expenses, incomes, and savings plan.
    /// </summary>
    /// <param name="id">The ID of the financial plan to delete.</param>
    /// <returns>True if the deletion is successful, false if the plan is not found or an error occurs.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    public async Task<bool> DeleteFinancialPlanAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

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

            if (plan.SavingsPlan is not null)
            {
                await DeleteSavingsPlanAsync(plan.SavingsPlan.Id);
            }

            _db.FinancialPlans.Remove(plan);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    #endregion



    #region BankAccount

    /// <summary>
    /// Retrieves a bank account by its ID.
    /// </summary>
    /// <param name="id">The ID of the bank account to retrieve.</param>
    /// <returns>A <see cref="BankAccountDTO"/> representing the bank account, or null if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    public async Task<BankAccountDTO?> GetBankAccountAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        var account = await _db.BankAccounts.FindAsync(id);
        return account?.ToBankAccountDTO();
    }

    /// <summary>
    /// Retrieves a list of all bank accounts.
    /// </summary>
    /// <returns>A list of <see cref="BankAccountDTO"/> objects representing all bank accounts.</returns>
    public async Task<List<BankAccountDTO>> GetBankAccountListAsync()
    {
        var accounts = await _db.BankAccounts.ToListAsync();

        var output = new List<BankAccountDTO>(accounts.Count);
        accounts.ForEach(x => output.Add(x.ToBankAccountDTO()));

        return output;
    }

    ///// <summary>
    ///// Creates or updates a bank account based on the provided DTO.
    ///// </summary>
    ///// <param name="dto">The <see cref="BankAccountDTO"/> containing the bank account data.</param>
    ///// <returns>True if the operation is successful, false otherwise.</returns>
    ///// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    //public async Task<bool> CreateOrUpdateBankAccountAsync(BankAccountDTO dto, int financialPlanId)
    //{
    //    ArgumentNullException.ThrowIfNull(dto);
    //    ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(financialPlanId, 0);

    //    var isNewAccount = await _db.BankAccounts.FindAsync(dto.Id) is null;

    //    return isNewAccount ? await CreateBankAccountAsync(dto, financialPlanId)
    //                        : await UpdateBankAccountAsync(dto);
    //}

    /// <summary>
    /// Creates a new bank account and associates it with the specified financial plan.
    /// </summary>
    /// <param name="dto">The <see cref="BankAccountDTO"/> containing the bank account data.</param>
    /// <param name="financialPlanId">The ID of the <see cref="FinancialPlanModel"/> to which the bank account will be associated.</param>
    /// <returns>True if the creation and association are successful, false if an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="financialPlanId"/> is less than or equal to zero.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    public async Task<bool> CreateBankAccountAsync(BankAccountDTO dto, int financialPlanId)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(financialPlanId, 0);

        var bankAccountModel = dto.ToBankAccountModel();

        bankAccountModel.DateCreated = DateTime.Now;
        bankAccountModel.DateModified = DateTime.Now;

        var financialPlan = await _db.FinancialPlans
            .Include(x => x.BankAccounts)
            .FirstAsync(x => x.Id == financialPlanId);

        financialPlan.BankAccounts.Add(bankAccountModel);
        financialPlan.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Updates an existing bank account with the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="BankAccountDTO"/> containing the updated bank account data.</param>
    /// <returns>True if the update is successful, false if the account is not found or an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    public async Task<bool> UpdateBankAccountAsync(BankAccountDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var model = dto.ToBankAccountModel();

        var oldAccount = await _db.BankAccounts.FindAsync(dto.Id);
        if (oldAccount is null)
        {
            return false;
        }

        oldAccount.BankName = dto.BankName;
        oldAccount.Description = dto.Description;
        oldAccount.IBAN = dto.IBAN;
        oldAccount.BIC = dto.BIC;
        oldAccount.DateModified = DateTime.Now;

        return await _db.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Deletes a bank account by its ID.
    /// </summary>
    /// <param name="id">The ID of the bank account to delete.</param>
    /// <returns>True if the deletion is successful, false if the account is not found or an error occurs.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    public async Task<bool> DeleteBankAccountAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        var account = await _db.BankAccounts.FindAsync(id);
        if (account is null)
        {
            return false;
        }

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            var expenses = await _db.ExpenseEntries.Where((x) => x.AssociatedBankAccount != null &&
                                                                 x.AssociatedBankAccount.Id == account.Id)
                                                                 .ToListAsync();

            // TODO: needs to be checked in DB if this actually works.
            expenses.ForEach(x => x.AssociatedBankAccount = null);

            _db.BankAccounts.Remove(account);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    #endregion



    #region ExpenseEntry

    /// <summary>
    /// Retrieves an expense entry by its ID, including the associated bank account.
    /// </summary>
    /// <param name="id">The ID of the expense entry to retrieve.</param>
    /// <returns>A <see cref="ExpenseEntryDTO"/> representing the expense entry, or null if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    public async Task<ExpenseEntryDTO?> GetExpenseEntryAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        var output = await _db.ExpenseEntries
            .Include(x => x.AssociatedBankAccount)
            .SingleOrDefaultAsync(x => x.Id == id);

        return output?.ToExpenseEntryDTO();
    }

    /// <summary>
    /// Retrieves a list of all expense entries, including their associated bank accounts.
    /// </summary>
    /// <returns>A list of <see cref="ExpenseEntryDTO"/> objects representing all expense entries.</returns>
    public async Task<List<ExpenseEntryDTO>> GetExpenseEntryListAsync()
    {
        var entries = await _db.ExpenseEntries
            .Include(x => x.AssociatedBankAccount)
            .ToListAsync();

        var output = new List<ExpenseEntryDTO>(entries.Count);
        entries.ForEach(x => output.Add(x.ToExpenseEntryDTO()));

        return output;
    }

    /// <summary>
    /// Creates or updates an expense entry based on the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="ExpenseEntryDTO"/> containing the expense entry data.</param>
    /// <returns>True if the operation is successful, false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    public async Task<bool> CreateOrUpdateExpenseEntryAsync(ExpenseEntryDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var isNewExpense = await _db.ExpenseEntries.FindAsync(dto.Id) is null;

        return isNewExpense ? await CreateExpenseEntryAsync(dto)
                            : await UpdateExpenseEntryAsync(dto);
    }

    /// <summary>
    /// Creates a new expense entry from the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="ExpenseEntryDTO"/> containing the expense entry data.</param>
    /// <returns>True if the creation is successful, false if an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    private async Task<bool> CreateExpenseEntryAsync(ExpenseEntryDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

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
            throw;
        }
    }

    /// <summary>
    /// Updates an existing expense entry with the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="ExpenseEntryDTO"/> containing the updated expense entry data.</param>
    /// <returns>True if the update is successful, false if the expense entry is not found or an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    private async Task<bool> UpdateExpenseEntryAsync(ExpenseEntryDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var oldEntry = await _db.ExpenseEntries.FindAsync(dto.Id);
        if (oldEntry is null)
        {
            return false;
        }

        var newEntry = dto.ToExpenseEntryModel();

        oldEntry.Name = newEntry.Name;
        oldEntry.Description = newEntry.Description;
        oldEntry.Price = newEntry.Price;
        oldEntry.DateModified = DateTime.Now;

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            if (oldEntry.AssociatedBankAccount is not null)
            {
                if (newEntry.AssociatedBankAccount is null)
                {
                    //_db.Entry(oldEntry.AssociatedBankAccount).State = EntityState.Detached;
                    oldEntry.AssociatedBankAccount = null;
                }
                else if (newEntry.AssociatedBankAccount.Id != oldEntry.AssociatedBankAccount.Id)
                {
                    //_db.Entry(oldEntry.AssociatedBankAccount).State = EntityState.Detached;
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
            throw;
        }
    }

    /// <summary>
    /// Deletes an expense entry by its ID.
    /// </summary>
    /// <param name="id">The ID of the expense entry to delete.</param>
    /// <returns>True if the deletion is successful, false if the expense entry is not found or an error occurs.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    public async Task<bool> DeleteExpenseEntryAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

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
            throw;
        }
    }
    #endregion



    #region IncomeEntry

    /// <summary>
    /// Retrieves an income entry by its ID.
    /// </summary>
    /// <param name="id">The ID of the income entry to retrieve.</param>
    /// <returns>A <see cref="IncomeEntryDTO"/> representing the income entry, or null if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    public async Task<IncomeEntryDTO?> GetIncomeEntryAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        return (await _db.IncomeEntries.FindAsync(id))?.ToIncomeEntryDTO();
    }

    /// <summary>
    /// Retrieves a list of all income entries.
    /// </summary>
    /// <returns>A list of <see cref="IncomeEntryDTO"/> objects representing all income entries.</returns>
    public async Task<List<IncomeEntryDTO>> GetIncomeEntryListAsync()
    {
        var entries = await _db.IncomeEntries.ToListAsync();

        var output = new List<IncomeEntryDTO>(entries.Count);
        entries.ForEach(x => output.Add(x.ToIncomeEntryDTO()));

        return output;
    }

    /// <summary>
    /// Creates or updates an income entry based on the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="IncomeEntryDTO"/> containing the income entry data.</param>
    /// <returns>True if the operation is successful, false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    public async Task<bool> CreateOrUpdateIncomeEntryAsync(IncomeEntryDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var isNewEntry = await _db.IncomeEntries.FindAsync(dto.Id) is null;

        return isNewEntry ? await CreateIncomeEntryAsync(dto)
                          : await UpdateIncomeEntryAsync(dto);
    }

    /// <summary>
    /// Creates a new income entry from the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="IncomeEntryDTO"/> containing the income entry data.</param>
    /// <returns>True if the creation is successful, false if an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    private async Task<bool> CreateIncomeEntryAsync(IncomeEntryDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

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
            throw;
        }
    }

    /// <summary>
    /// Updates an existing income entry with the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="IncomeEntryDTO"/> containing the updated income entry data.</param>
    /// <returns>True if the update is successful, false if the income entry is not found or an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    private async Task<bool> UpdateIncomeEntryAsync(IncomeEntryDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var oldEntry = await _db.IncomeEntries.FindAsync(dto.Id);
        if (oldEntry is null)
        {
            return false;
        }

        var newEntry = dto.ToIncomeModel();

        oldEntry.Name = newEntry.Name;
        oldEntry.Description = newEntry.Description;
        oldEntry.TotalAmount = newEntry.TotalAmount;
        oldEntry.DateModified = DateTime.Now;

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
            throw;
        }
    }

    /// <summary>
    /// Deletes an income entry by its ID.
    /// </summary>
    /// <param name="id">The ID of the income entry to delete.</param>
    /// <returns>True if the deletion is successful, false if the income entry is not found or an error occurs.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    public async Task<bool> DeleteIncomeEntryAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

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
            throw;
        }
    }
    #endregion



    #region SavingsEntry

    /// <summary>
    /// Retrieves a savings entry by its ID.
    /// </summary>
    /// <param name="id">The ID of the savings entry to retrieve.</param>
    /// <returns>A <see cref="SavingsEntryDTO"/> representing the savings entry, or null if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    public async Task<SavingsEntryDTO?> GetSavingsEntryAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        return (await _db.SavingsEntries.FindAsync(id))?.ToSavingsEntryDTO();
    }

    /// <summary>
    /// Retrieves a list of all savings entries.
    /// </summary>
    /// <returns>A list of <see cref="SavingsEntryDTO"/> objects representing all savings entries.</returns>
    public async Task<List<SavingsEntryDTO>> GetSavingsEntryListAsync()
    {
        var entries = await _db.SavingsEntries.ToListAsync();

        var output = new List<SavingsEntryDTO>(entries.Count);
        entries.ForEach(x => output.Add(x.ToSavingsEntryDTO()));

        return output;
    }

    /// <summary>
    /// Creates or updates a savings entry based on the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="SavingsEntryDTO"/> containing the savings entry data.</param>
    /// <returns>True if the operation is successful, false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    public async Task<bool> CreateOrUpdateSavingsEntryAsync(SavingsEntryDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var isNewEntry = await _db.SavingsEntries.FindAsync(dto.Id) is null;

        return isNewEntry ? await CreateSavingsEntryAsync(dto)
                          : await UpdateSavingsEntryAsync(dto);
    }

    /// <summary>
    /// Creates a new savings entry from the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="SavingsEntryDTO"/> containing the savings entry data.</param>
    /// <returns>True if the creation is successful, false if an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    private async Task<bool> CreateSavingsEntryAsync(SavingsEntryDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

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
            throw;
        }
    }

    /// <summary>
    /// Updates an existing savings entry with the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="SavingsEntryDTO"/> containing the updated savings entry data.</param>
    /// <returns>True if the update is successful, false if the savings entry is not found or an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    private async Task<bool> UpdateSavingsEntryAsync(SavingsEntryDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var oldEntry = await _db.SavingsEntries.FindAsync(dto.Id);
        if (oldEntry is null)
        {
            return false;
        }

        var newEntry = dto.ToSavingsEntryModel();

        oldEntry.Name = newEntry.Name;
        oldEntry.Description = newEntry.Description;
        oldEntry.Price = newEntry.Price;
        oldEntry.DateModified = DateTime.Now;

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
            throw;
        }
    }

    /// <summary>
    /// Deletes a savings entry by its ID.
    /// </summary>
    /// <param name="id">The ID of the savings entry to delete.</param>
    /// <returns>True if the deletion is successful, false if the savings entry is not found or an error occurs.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    public async Task<bool> DeleteSavingsEntryAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

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
            throw;
        }
    }
    #endregion



    #region SavingsPlan

    /// <summary>
    /// Retrieves a savings plan by its ID, including related savings entries.
    /// </summary>
    /// <param name="id">The ID of the savings plan to retrieve.</param>
    /// <returns>A <see cref="SavingsPlanDTO"/> representing the savings plan, or null if not found.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    public async Task<SavingsPlanDTO?> GetSavingsPlanAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        var plan = await _db.SavingsPlans
            .Include(s => s.SavingsEntries)
            .SingleOrDefaultAsync(x => x.Id == id);

        return plan?.ToSavingsPlanDTO();
    }

    /// <summary>
    /// Retrieves a list of all savings plans, including their related savings entries.
    /// </summary>
    /// <returns>A list of <see cref="SavingsPlanDTO"/> objects representing all savings plans.</returns>
    public async Task<List<SavingsPlanDTO>> GetSavingsPlanListAsync()
    {
        var plans = await _db.SavingsPlans
            .Include(s => s.SavingsEntries)
            .ToListAsync();

        var output = new List<SavingsPlanDTO>(plans.Count);
        plans.ForEach(x => output.Add(x.ToSavingsPlanDTO()));

        return output;
    }

    /// <summary>
    /// Creates or updates a savings plan based on the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="SavingsPlanDTO"/> containing the savings plan data.</param>
    /// <returns>True if the operation is successful, false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    public async Task<bool> CreateOrUpdateSavingsPlanAsync(SavingsPlanDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var isNewPlan = await _db.SavingsPlans.FindAsync(dto.Id) is null;

        return isNewPlan ? await CreateSavingsPlanAsync(dto)
                         : await UpdateSavingsPlanAsync(dto);
    }

    /// <summary>
    /// Creates a new savings plan from the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="SavingsPlanDTO"/> containing the savings plan data.</param>
    /// <returns>True if the creation is successful, false if an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    private async Task<bool> CreateSavingsPlanAsync(SavingsPlanDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

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
            throw;
        }
    }

    /// <summary>
    /// Updates an existing savings plan with the provided DTO.
    /// </summary>
    /// <param name="dto">The <see cref="SavingsPlanDTO"/> containing the updated savings plan data.</param>
    /// <returns>True if the update is successful, false if the savings plan is not found or an error occurs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    private async Task<bool> UpdateSavingsPlanAsync(SavingsPlanDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

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
                var entriesToDelete = oldPlan.SavingsEntries.Except(newPlan.SavingsEntries).ToList();
                entriesToDelete.ForEach(x => oldPlan.SavingsEntries.Remove(x));

                var entriesToAdd = newPlan.SavingsEntries.Except(oldPlan.SavingsEntries).ToList();
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
            throw;
        }
    }

    /// <summary>
    /// Deletes a savings plan by its ID, including related savings entries.
    /// </summary>
    /// <param name="id">The ID of the savings plan to delete.</param>
    /// <returns>True if the deletion is successful, false if the savings plan is not found or an error occurs.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="id"/> is less than or equal to zero.</exception>
    /// <exception cref="Exception">Thrown when a general error occurs during the database operation.</exception>
    public async Task<bool> DeleteSavingsPlanAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(id, 0);

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            var savingsPlan = await _db.SavingsPlans
                .Include(s => s.SavingsEntries)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (savingsPlan is null)
            {
                await transaction.RollbackAsync();
                return false;
            }

            var financialPlan = await _db.FinancialPlans.SingleOrDefaultAsync((x) => x.SavingsPlan != null &&
                                                                                     x.SavingsPlan.Id == savingsPlan.Id);
            financialPlan!.SavingsPlan = null;

            _db.SavingsEntries.RemoveRange(savingsPlan.SavingsEntries);
            _db.SavingsPlans.Remove(savingsPlan);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    #endregion
}