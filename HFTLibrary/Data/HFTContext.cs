using HFTLibrary.Models;

using Microsoft.EntityFrameworkCore;

namespace HFTLibrary.API;

public class HFTContext : DbContext
{
    public string DbPath { get; }

    public DbSet<BankAccountModel> BankAccounts { get; set; }
    public DbSet<ExpenseEntryModel> ExpenseEntries { get; set; }
    public DbSet<FinancialPlanModel> FinancialPlans { get; set; }
    public DbSet<IncomeEntryModel> IncomeEntries { get; set; }
    public DbSet<SavingsEntryModel> SavingsEntries { get; set; }
    public DbSet<SavingsPlanModel> SavingsPlans { get; set; }

    public HFTContext(DbContextOptions<HFTContext> options) : base(options)
    {
        var folder = @"C:\Temp\DB\";
        DbPath = System.IO.Path.Join(folder, "HFTApp.db");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
      => options.UseSqlite($"Data Source={DbPath}");
}