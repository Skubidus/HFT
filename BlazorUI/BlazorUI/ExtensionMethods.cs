using BlazorUI.Components.Pages;
using BlazorUI.ViewModels;

using HFTLibrary.DTOs;

namespace BlazorUI;

public static class ExtensionMethods
{
    // bank account
    public static BankAccountViewModel ToBankAccountViewModel(this BankAccountDTO dto)
    {
        return new BankAccountViewModel
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

    public static BankAccountDTO ToBankAccountDTO(this BankAccountViewModel model)
    {
        return new BankAccountDTO
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            IBAN = model.IBAN,
            BIC = model.BIC,
            DateCreated = model.DateCreated,
            DateModified = model.DateModified
        };
    }

    // financial plan
    public static FinancialPlanViewModel ToFinancialPlanViewModel(this FinancialPlanDTO dto)
    {
        List<BankAccountViewModel> bankAccounts = [];
        dto.BankAccounts.ToList().ForEach(x => bankAccounts.Add(x.ToBankAccountViewModel()));

        List<ExpenseEntryViewModel> expenses = [];
        dto.Expenses.ToList().ForEach(x => expenses.Add(x.ToExpenseEntryViewModel()));

        List<IncomeEntryViewModel> incomes = [];
        dto.Incomes.ToList().ForEach(x => incomes.Add(x.ToIncomeEntryViewModel()));

        return new FinancialPlanViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            BankAccounts = bankAccounts,
            Expenses = expenses,
            Incomes = incomes,
            SavingsPlan = dto.SavingsPlan?.ToSavingsPlanViewModel(),
            DateCreated = dto.DateCreated,
            DateModified = dto.DateModified
        };
    }

    public static FinancialPlanDTO ToFinancialPlanDTO(this FinancialPlanViewModel model)
    {
        List<BankAccountDTO> bankAccounts = [];
        model.BankAccounts.ToList().ForEach(x => bankAccounts.Add(x.ToBankAccountDTO()));

        List<ExpenseEntryDTO> expenses = [];
        model.Expenses.ToList().ForEach(x => expenses.Add(x.ToExpenseEntryDTO()));

        List<IncomeEntryDTO> incomes = [];
        model.Incomes.ToList().ForEach(x => incomes.Add(x.ToIncomeEntryDTO()));

        return new FinancialPlanDTO
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
    }

    // expense entry
    public static ExpenseEntryViewModel ToExpenseEntryViewModel(this ExpenseEntryDTO dto)
    {
        return new ExpenseEntryViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            AssociatedBankAccount = dto.AssociatedBankAccount?.ToBankAccountViewModel(),
            DateCreated = dto.DateCreated,
            DateModified = dto.DateModified
        };
    }

    public static ExpenseEntryDTO ToExpenseEntryDTO(this ExpenseEntryViewModel model)
    {
        return new ExpenseEntryDTO
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            AssociatedBankAccount = model.AssociatedBankAccount?.ToBankAccountDTO(),
            DateCreated = model.DateCreated,
            DateModified = model.DateModified
        };
    }

    // income entry
    public static IncomeEntryViewModel ToIncomeEntryViewModel(this IncomeEntryDTO dto)
    {
        return new IncomeEntryViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            TotalAmount = dto.TotalAmount,
            DateCreated = dto.DateCreated,
            DateModified = dto.DateModified
        };
    }

    public static IncomeEntryDTO ToIncomeEntryDTO(this IncomeEntryViewModel model)
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

    // savings entry
    public static SavingsEntryViewModel ToSavingsEntryViewModel(this SavingsEntryDTO dto)
    {
        return new SavingsEntryViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            DateCreated = dto.DateCreated,
            DateModified = dto.DateModified
        };
    }

    public static SavingsEntryDTO ToSavingsEntryDTO(this SavingsEntryViewModel model)
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

    // savings plan
    public static SavingsPlanViewModel ToSavingsPlanViewModel(this SavingsPlanDTO dto)
    {
        List<SavingsEntryViewModel> savingsEntries = [];
        dto.SavingsEntries.ToList().ForEach(x => savingsEntries.Add(x.ToSavingsEntryViewModel()));

        return new SavingsPlanViewModel
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

    public static SavingsPlanDTO ToSavingsPlanDTO(this SavingsPlanViewModel model)
    {
        List<SavingsEntryDTO> savingsEntries = [];
        model.SavingsEntries.ForEach(x => savingsEntries.Add(x.ToSavingsEntryDTO()));

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
}