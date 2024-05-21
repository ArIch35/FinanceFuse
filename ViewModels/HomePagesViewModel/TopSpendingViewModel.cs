using System;
using System.Collections.Generic;
using FinanceFuse.Models;

namespace FinanceFuse.ViewModels.HomePagesViewModel;

public class CategorySum(Category category, double total)
{
    public Category Category { get; } = category;
    public double Total { get; } = total;
    public int Percentage { get; set; }
}

public class TopSpendingViewModel(IEnumerable<CategorySum> categorySums)
{
    public IEnumerable<CategorySum> CategorySums { get; set; } = categorySums;
    public DateTime CurrentDate { get; set; } = DateTime.Now;
}