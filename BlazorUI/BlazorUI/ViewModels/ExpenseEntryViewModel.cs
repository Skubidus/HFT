﻿using System.ComponentModel.DataAnnotations;

namespace BlazorUI.ViewModels;

public class ExpenseEntryViewModel
{

    public int Id { get; init; }

    [StringLength(100)]
    public required string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0, 99999999.99)]
    public required decimal Price { get => _price; set => _price = value; }
    private decimal _price;

    public string PriceString
    {
        get => Price.ToString("N2");
        set
        {
            value = value.Replace(".", "");
            _ = decimal.TryParse(value, out _price);
        }

    }

    public BankAccountViewModel? AssociatedBankAccount { get; set; }
    public required DateTime DateCreated { get; set; }
    public required DateTime DateModified { get; set; }
}