﻿// <auto-generated />
using System;
using HFTLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HFTLibrary.Migrations
{
    [DbContext(typeof(HFTContext))]
    partial class HFTContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("HFTLibrary.Models.BankAccountModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BIC")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("FinancialPlanModelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IBAN")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FinancialPlanModelId");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("HFTLibrary.Models.ExpenseEntryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AssociatedBankAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("FinancialPlanModelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AssociatedBankAccountId");

                    b.HasIndex("FinancialPlanModelId");

                    b.ToTable("ExpenseEntries");
                });

            modelBuilder.Entity("HFTLibrary.Models.FinancialPlanModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("SavingsPlanId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SavingsPlanId");

                    b.ToTable("FinancialPlans");
                });

            modelBuilder.Entity("HFTLibrary.Models.IncomeEntryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("FinancialPlanModelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FinancialPlanModelId");

                    b.ToTable("IncomeEntries");
                });

            modelBuilder.Entity("HFTLibrary.Models.SavingsEntryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SavingsPlanModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SavingsPlanModelId");

                    b.ToTable("SavingsEntries");
                });

            modelBuilder.Entity("HFTLibrary.Models.SavingsPlanModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("DurationInMonths")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SavingsPlans");
                });

            modelBuilder.Entity("HFTLibrary.Models.BankAccountModel", b =>
                {
                    b.HasOne("HFTLibrary.Models.FinancialPlanModel", null)
                        .WithMany("BankAccounts")
                        .HasForeignKey("FinancialPlanModelId");
                });

            modelBuilder.Entity("HFTLibrary.Models.ExpenseEntryModel", b =>
                {
                    b.HasOne("HFTLibrary.Models.BankAccountModel", "AssociatedBankAccount")
                        .WithMany()
                        .HasForeignKey("AssociatedBankAccountId");

                    b.HasOne("HFTLibrary.Models.FinancialPlanModel", null)
                        .WithMany("Expenses")
                        .HasForeignKey("FinancialPlanModelId");

                    b.Navigation("AssociatedBankAccount");
                });

            modelBuilder.Entity("HFTLibrary.Models.FinancialPlanModel", b =>
                {
                    b.HasOne("HFTLibrary.Models.SavingsPlanModel", "SavingsPlan")
                        .WithMany()
                        .HasForeignKey("SavingsPlanId");

                    b.Navigation("SavingsPlan");
                });

            modelBuilder.Entity("HFTLibrary.Models.IncomeEntryModel", b =>
                {
                    b.HasOne("HFTLibrary.Models.FinancialPlanModel", null)
                        .WithMany("Incomes")
                        .HasForeignKey("FinancialPlanModelId");
                });

            modelBuilder.Entity("HFTLibrary.Models.SavingsEntryModel", b =>
                {
                    b.HasOne("HFTLibrary.Models.SavingsPlanModel", null)
                        .WithMany("SavingsEntries")
                        .HasForeignKey("SavingsPlanModelId");
                });

            modelBuilder.Entity("HFTLibrary.Models.FinancialPlanModel", b =>
                {
                    b.Navigation("BankAccounts");

                    b.Navigation("Expenses");

                    b.Navigation("Incomes");
                });

            modelBuilder.Entity("HFTLibrary.Models.SavingsPlanModel", b =>
                {
                    b.Navigation("SavingsEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
