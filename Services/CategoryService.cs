using System.Collections.Generic;
using FinanceFuse.Models;

namespace FinanceFuse.Services;

public static class CategoryService
{
    public static readonly Category NoCategory = new("0", CategoryType.None, "No Category", "Assets/Categories/no-category-icon.png");
    public static List<Category> Categories { get; }

    static CategoryService()
    {
        Categories = InitCategories();
    }

    private static List<Category> InitCategories()
    {
        return new List<Category>()
        {
            new("1", CategoryType.Expense, "Bills & Utilities", "Assets/Categories/Bills/bills-icon.png")
            {
                SubCategories = new List<Category>()
                {
                    new("2", CategoryType.Expense, "Electric Bills", "Assets/Categories/Bills/electric-bills-icon.png"),
                    new("3", CategoryType.Expense, "Gas Bills", "Assets/Categories/Bills/gas-bills-icon.png"),
                }
            },
            new("4", CategoryType.Expense, "Education", "Assets/Categories/Education/education-icon.png")
            {
                SubCategories = new List<Category>()
                {
                    new("5", CategoryType.Expense, "College Fee", "Assets/Categories/Education/college-fee-icon.png")
                }
            },
            new("5", CategoryType.Income, "Salary", "Assets/Categories/Income/salary-icon.png")
        };
    }
}