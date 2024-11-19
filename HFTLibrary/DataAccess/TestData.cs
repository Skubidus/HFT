using HFTLibrary.Models;

namespace HFTLibrary.DataAccess;
public class TestData
{
    public List<FinancialPlanModel> FinancialPlans { get; } = [];

    public TestData()
    {
        #region plan 1
        List<BankAccountModel> bankAccounts =
        [
            new BankAccountModel
            {
                Id = 1,
                Name = "Bank Account 1",
                Description = "Bank Account 1 Test",
                IBAN = "DE12500105170648489890",
                BIC = "50010517",
                DateCreated = new DateTime(2024, 2, 10),
                DateModified = new DateTime(2024, 2, 10)
            },

            new BankAccountModel
            {
                Id = 2,
                Name = "Bank Account 2",
                Description = "Bank Account 2 Test",
                IBAN = "DE12500105170648489785",
                BIC = "50010555",
                DateCreated = new DateTime(2024, 5, 10),
                DateModified = new DateTime(2024, 5, 13)
            }
        ];

        List<ExpenseEntryModel> expenses =
        [
            new ExpenseEntryModel
            {
                Id = 1,
                Name = "Expense 1",
                Description = "Expense Test 1",
                Price = 299.99m,
                AssociatedBankAccount = bankAccounts[0],
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
            new ExpenseEntryModel
            {
                Id = 2,
                Name = "Expense 2",
                Description = "Expense Test 2",
                Price = 149.50m,
                AssociatedBankAccount = bankAccounts[0],
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
           new ExpenseEntryModel
            {
                Id = 3,
                Name = "Expense 3",
                Description = "Expense Test 3",
                Price = 99.90m,
                AssociatedBankAccount = bankAccounts[1],
                DateCreated = new DateTime(2024, 10, 20),
                DateModified = new DateTime(2024, 10, 22)
            }
        ];

        List<IncomeEntryModel> incomes =
        [
            new IncomeEntryModel
            {
                Id = 1,
                Name = "Income 1",
                Description = "Income Test 1",
                TotalAmount = 299.99m,
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
            new IncomeEntryModel
            {
                Id = 2,
                Name = "Income 2",
                Description = "Income Test 2",
                TotalAmount = 149.50m,
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
           new IncomeEntryModel
            {
                Id = 3,
                Name = "Income 3",
                Description = "Income Test 3",
                TotalAmount = 99.90m,
                DateCreated = new DateTime(2024, 10, 20),
                DateModified = new DateTime(2024, 10, 22)
            }
        ];

        List<SavingsEntryModel> savingsEntries =
        [
            new SavingsEntryModel
            {
                Id = 1,
                Name = "Savings Entry 1",
                Description = "Savings Entry Test 1",
                Price = 500.00m,
                DateCreated = new DateTime(2024, 10, 20),
                DateModified = new DateTime(2024, 10, 22)
            },
            new SavingsEntryModel
            {
                Id = 2,
                Name = "Savings Entry 2",
                Description = "Savings Entry Test 2",
                Price = 700.00m,
                DateCreated = new DateTime(2024, 10, 22),
                DateModified = new DateTime(2024, 10, 22)
            },
            new SavingsEntryModel
            {
                Id = 3,
                Name = "Savings Entry 3",
                Description = "Savings Entry Test 3",
                Price = 100.00m,
                DateCreated = new DateTime(2024, 3, 18),
                DateModified = new DateTime(2024, 3, 18)
            }
        ];

        var savingsPlan = new SavingsPlanModel
        {
            Id = 1,
            Name = "Savings Plan 1",
            DurationInMonths = 36,
            SavingsEntries = savingsEntries,
            Description = "Savings Plan 1 Test",
            DateCreated = new DateTime(2022, 11, 2),
            DateModified = new DateTime(2022, 11, 2)
        };
        
        var plan1 = new FinancialPlanModel
        {
            Id = 1,
            Name = "Finance Plan 01",
            Description = "First financial plan test.",
            BankAccounts = bankAccounts,
            Expenses = expenses,
            Incomes = incomes,
            SavingsPlan = savingsPlan,
            DateCreated = DateTime.Now,
            DateModified = DateTime.Now
        };
        #endregion

        #region plan 2
        List<BankAccountModel> bankAccounts2 =
        [
            new BankAccountModel
            {
                Id = 11,
                Name = "Bank Account 11",
                Description = "Bank Account 11 Test",
                IBAN = "DE12500105170648489890",
                BIC = "50010517",
                DateCreated = new DateTime(2024, 2, 10),
                DateModified = new DateTime(2024, 2, 10)
            },

            new BankAccountModel
            {
                Id = 12,
                Name = "Bank Account 12",
                Description = "Bank Account 12 Test",
                IBAN = "DE12500105170648489785",
                BIC = "50010555",
                DateCreated = new DateTime(2024, 5, 10),
                DateModified = new DateTime(2024, 5, 13)
            }
        ];

        List<ExpenseEntryModel> expenses2 =
        [
            new ExpenseEntryModel
            {
                Id = 11,
                Name = "Expense 11",
                Description = "Expense Test 11",
                Price = 299.99m,
                AssociatedBankAccount = bankAccounts2[0],
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
            new ExpenseEntryModel
            {
                Id = 12,
                Name = "Expense 12",
                Description = "Expense Test 12",
                Price = 149.50m,
                AssociatedBankAccount = bankAccounts2[0],
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
           new ExpenseEntryModel
            {
                Id = 13,
                Name = "Expense 13",
                Description = "Expense Test 13",
                Price = 99.90m,
                AssociatedBankAccount = bankAccounts2[1],
                DateCreated = new DateTime(2024, 10, 20),
                DateModified = new DateTime(2024, 10, 22)
            }
        ];

        List<IncomeEntryModel> incomes2 =
        [
            new IncomeEntryModel
            {
                Id = 11,
                Name = "Income 11",
                Description = "Income Test 11",
                TotalAmount = 299.99m,
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
            new IncomeEntryModel
            {
                Id = 12,
                Name = "Income 12",
                Description = "Income Test 12",
                TotalAmount = 149.50m,
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
           new IncomeEntryModel
            {
                Id = 13,
                Name = "Income 13",
                Description = "Income Test 13",
                TotalAmount = 99.90m,
                DateCreated = new DateTime(2024, 10, 20),
                DateModified = new DateTime(2024, 10, 22)
            }
        ];

        List<SavingsEntryModel> savingsEntries2 =
        [
            new SavingsEntryModel
            {
                Id = 11,
                Name = "Savings Entry 11",
                Description = "Savings Entry Test 11",
                Price = 500.00m,
                DateCreated = new DateTime(2024, 10, 20),
                DateModified = new DateTime(2024, 10, 22)
            },
            new SavingsEntryModel
            {
                Id = 12,
                Name = "Savings Entry 12",
                Description = "Savings Entry Test 12",
                Price = 700.00m,
                DateCreated = new DateTime(2024, 10, 22),
                DateModified = new DateTime(2024, 10, 22)
            },
            new SavingsEntryModel
            {
                Id = 13,
                Name = "Savings Entry 13",
                Description = "Savings Entry Test 13",
                Price = 100.00m,
                DateCreated = new DateTime(2024, 3, 18),
                DateModified = new DateTime(2024, 3, 18)
            }
        ];

        var savingsPlan2 = new SavingsPlanModel
        {
            Id = 11,
            Name = "Savings Plan 2",
            DurationInMonths = 24,
            SavingsEntries = savingsEntries2,
            Description = "Savings Plan 2 Test",
            DateCreated = new DateTime(2022, 11, 2),
            DateModified = new DateTime(2022, 11, 2)
        };

        var plan2 = new FinancialPlanModel
        {
            Id = 2,
            Name = "Finance Plan 02",
            Description = "Second financial plan test.",
            BankAccounts = bankAccounts2,
            Expenses = expenses2,
            Incomes = incomes2,
            SavingsPlan = savingsPlan2,
            DateCreated = DateTime.Now,
            DateModified = DateTime.Now
        };
        #endregion

        #region plan 3
        List<BankAccountModel> bankAccounts3 =
        [
            new BankAccountModel
            {
                Id = 21,
                Name = "Bank Account 21",
                Description = "Bank Account 21 Test",
                IBAN = "DE12500105170648489890",
                BIC = "50010517",
                DateCreated = new DateTime(2024, 2, 10),
                DateModified = new DateTime(2024, 2, 10)
            },

            new BankAccountModel
            {
                Id = 22,
                Name = "Bank Account 22",
                Description = "Bank Account 22 Test",
                IBAN = "DE12500105170648489785",
                BIC = "50010555",
                DateCreated = new DateTime(2024, 5, 10),
                DateModified = new DateTime(2024, 5, 13)
            }
        ];

        List<ExpenseEntryModel> expenses3 =
        [
            new ExpenseEntryModel
            {
                Id = 21,
                Name = "Expense 21",
                Description = "Expense Test 21",
                Price = 299.99m,
                AssociatedBankAccount = bankAccounts3[0],
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
            new ExpenseEntryModel
            {
                Id = 22,
                Name = "Expense 22",
                Description = "Expense Test 22",
                Price = 149.50m,
                AssociatedBankAccount = bankAccounts3[0],
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
           new ExpenseEntryModel
            {
                Id = 23,
                Name = "Expense 23",
                Description = "Expense Test 23",
                Price = 99.90m,
                AssociatedBankAccount = bankAccounts3[1],
                DateCreated = new DateTime(2024, 10, 20),
                DateModified = new DateTime(2024, 10, 22)
            }
        ];

        List<IncomeEntryModel> incomes3 =
        [
            new IncomeEntryModel
            {
                Id = 21,
                Name = "Income 21",
                Description = "Income Test 21",
                TotalAmount = 299.99m,
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
            new IncomeEntryModel
            {
                Id = 22,
                Name = "Income 22",
                Description = "Income Test 22",
                TotalAmount = 149.50m,
                DateCreated = new DateTime(2024, 7, 23),
                DateModified = new DateTime(2024, 7, 23)
            },
           new IncomeEntryModel
            {
                Id = 23,
                Name = "Income 23",
                Description = "Income Test 23",
                TotalAmount = 99.90m,
                DateCreated = new DateTime(2024, 10, 20),
                DateModified = new DateTime(2024, 10, 22)
            }
        ];

        List<SavingsEntryModel> savingsEntries3 =
        [
            new SavingsEntryModel
            {
                Id = 21,
                Name = "Savings Entry 21",
                Description = "Savings Entry Test 21",
                Price = 500.00m,
                DateCreated = new DateTime(2024, 10, 20),
                DateModified = new DateTime(2024, 10, 22)
            },
            new SavingsEntryModel
            {
                Id = 22,
                Name = "Savings Entry 22",
                Description = "Savings Entry Test 22",
                Price = 700.00m,
                DateCreated = new DateTime(2024, 10, 22),
                DateModified = new DateTime(2024, 10, 22)
            },
            new SavingsEntryModel
            {
                Id = 23,
                Name = "Savings Entry 23",
                Description = "Savings Entry Test 23",
                Price = 100.00m,
                DateCreated = new DateTime(2024, 3, 18),
                DateModified = new DateTime(2024, 3, 18)
            }
        ];

        var savingsPlan3 = new SavingsPlanModel
        {
            Id = 21,
            Name = "Savings Plan 3",
            DurationInMonths = 12,
            SavingsEntries = savingsEntries3,
            Description = "Savings Plan 3 Test",
            DateCreated = new DateTime(2023, 11, 2),
            DateModified = new DateTime(2023, 11, 2)
        };

        var plan3 = new FinancialPlanModel
        {
            Id = 3,
            Name = "Finance Plan 03",
            Description = "Third financial plan test.",
            BankAccounts = bankAccounts3,
            Expenses = expenses3,
            Incomes = incomes3,
            SavingsPlan = savingsPlan3,
            DateCreated = DateTime.Now,
            DateModified = DateTime.Now
        };
        #endregion

        FinancialPlans.Add(plan1);
        FinancialPlans.Add(plan2);
        FinancialPlans.Add(plan3);
    }
}