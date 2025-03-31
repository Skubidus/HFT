using HFTLibrary.DTOs;
using HFTLibrary.Models;

namespace HFTLibrary.Logic;
public static class ExtensionMethods
{
    public static FinancialPlanDTO ToFinancialPlanDTO(this FinancialPlanModel model)
    {
        var bankAccounts = new List<BankAccountDTO>();
        model.BankAccounts?.ForEach(x => bankAccounts.Add(x.ToBankAccountDTO()));

        var expenses = new List<ExpenseEntryDTO>();
        model.Expenses?.ForEach(x => expenses.Add(x.ToExpenseEntryDTO()));

        var incomes = new List<IncomeEntryDTO>();
        model.Incomes?.ForEach(x => incomes.Add(x.ToIncomeEntryDTO()));

        var output = new FinancialPlanDTO
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,

            BankAccounts = bankAccounts,
            Expenses = expenses,
            Incomes = incomes,

            SavingsPlan = model.SavingsPlan?.ToSavingsPlanDTO(),

            DateCreated = model.DateCreated,
            DateModified = model.DateModified
        };

        return output;
    }

    public static FinancialPlanModel ToFinancialPlanModel(this FinancialPlanDTO dto)
    {
        var bankAccounts = new List<BankAccountModel>();
        dto.BankAccounts?.ToList().ForEach(x => bankAccounts.Add(x.ToBankAccountModel()));

        var expenses = new List<ExpenseEntryModel>();
        dto.Expenses?.ToList().ForEach(x => expenses.Add(x.ToExpenseEntryModel()));

        var incomes = new List<IncomeEntryModel>();
        dto.Incomes?.ToList().ForEach(x => incomes.Add(x.ToIncomeModel()));

        var output = new FinancialPlanModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,

            BankAccounts = bankAccounts,
            Expenses = expenses,
            Incomes = incomes,

            SavingsPlan = dto.SavingsPlan?.ToSavingsPlanModel(),

            DateCreated = dto.DateCreated,
            DateModified = dto.DateModified
        };

        return output;
    }

    public static FinancialPlanLazyDTO ToFinancialPlanLazyDTO(this FinancialPlanModel model)
    {
        return new FinancialPlanLazyDTO
        {
            Id = model.Id,
            Name = model.Name
        };
    }

    public static BankAccountDTO ToBankAccountDTO(this BankAccountModel model)
    {
        return new BankAccountDTO
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,

            IBAN = model.IBAN,
            BIC = model.BIC,

            DateCreated = model.DateCreated,
            DateModified = model.DateModified,
        };
    }

    public static BankAccountModel ToBankAccountModel(this BankAccountDTO dto)
    {
        return new BankAccountModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,

            IBAN = dto.IBAN,
            BIC = dto.BIC,

            DateCreated = dto.DateCreated,
            DateModified = dto.DateModified
        };
    }

    public static ExpenseEntryDTO ToExpenseEntryDTO(this ExpenseEntryModel model)
    {
        return new ExpenseEntryDTO
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,

            AssociatedBankAccount = model.AssociatedBankAccount?.ToBankAccountDTO(),
            Price = model.Price,

            DateCreated = model.DateCreated,
            DateModified = model.DateModified
        };
    }

    public static ExpenseEntryModel ToExpenseEntryModel(this ExpenseEntryDTO dto)
    {
        return new ExpenseEntryModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,

            AssociatedBankAccount = dto.AssociatedBankAccount?.ToBankAccountModel(),
            Price = dto.Price,

            DateCreated = dto.DateCreated,
            DateModified = dto.DateModified
        };
    }

    public static IncomeEntryDTO ToIncomeEntryDTO(this IncomeEntryModel model)
    {
        return new IncomeEntryDTO
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,

            TotalAmount = model.TotalAmount,

            DateCreated = model.DateCreated,
            DateModified = model.DateModified
        };
    }

    public static IncomeEntryModel ToIncomeModel(this IncomeEntryDTO dto)
    {
        return new IncomeEntryModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,

            TotalAmount = dto.TotalAmount,

            DateCreated = dto.DateCreated,
            DateModified = dto.DateModified
        };
    }

    public static SavingsEntryDTO ToSavingsEntryDTO(this SavingsEntryModel model)
    {
        return new SavingsEntryDTO
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,

            Price = model.Price,

            DateCreated = model.DateCreated,
            DateModified = model.DateModified
        };
    }

    public static SavingsEntryModel ToSavingsEntryModel(this SavingsEntryDTO dto)
    {
        return new SavingsEntryModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,

            Price = dto.Price,

            DateCreated = dto.DateCreated,
            DateModified = dto.DateModified
        };
    }

    public static SavingsPlanDTO ToSavingsPlanDTO(this SavingsPlanModel model)
    {
        var savingsEntries = new List<SavingsEntryDTO>();
        model.SavingsEntries?.ForEach(x => savingsEntries.Add(x.ToSavingsEntryDTO()));

        return new SavingsPlanDTO
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,

            DurationInMonths = model.DurationInMonths,
            SavingsEntries = savingsEntries,

            DateCreated = model.DateCreated,
            DateModified = model.DateModified
        };
    }

    public static SavingsPlanModel ToSavingsPlanModel(this SavingsPlanDTO dto)
    {
        var savingsEntries = new List<SavingsEntryModel>();
        dto.SavingsEntries?.ToList().ForEach(x => savingsEntries.Add(x.ToSavingsEntryModel()));

        return new SavingsPlanModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            
            DurationInMonths = dto.DurationInMonths,
            SavingsEntries = savingsEntries,

            DateCreated = dto.DateCreated,
            DateModified = dto.DateModified
        };
    }
}