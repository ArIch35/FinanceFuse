using System.Collections.Generic;
using FinanceFuse.Models;

namespace FinanceFuse.Services;

public class CategoryService
{
    public static readonly Category NoCategory =
        new Category("0", CategoryType.None, "No Category", "Assets/Categories/no-category-icon.png");
        public List<Category> Categories { get; }
        private static CategoryService _instance = null!;
        private static readonly  object Locker = new();

        private CategoryService()
        {
            Categories = InitCategories();
        }
        
        public static CategoryService GetInstance()
        {
            if (_instance != null!)
                return _instance;

            lock (Locker)
            {
                _instance ??= new CategoryService();
            }

            return _instance;
        }

        private static List<Category> InitCategories()
        {
            return new List<Category>()
            {
                new Category("1", CategoryType.Expense, "Bills & Utilities", "Assets/Categories/Bills/bills-icon.png")
                {
                    SubCategories = new List<Category>()
                    {
                        new Category("2", CategoryType.Expense, "Electric Bills", "Assets/Categories/Bills/electric-bills-icon.png"),
                        new Category("3", CategoryType.Expense, "Gas Bills", "Assets/Categories/Bills/gas-bills-icon.png"),
                    }
                },
                new Category("4", CategoryType.Expense, "Education", "Assets/Categories/Education/education-icon.png")
                {
                    SubCategories = new List<Category>()
                    {
                        new Category("5", CategoryType.Expense, "College Fee", "Assets/Categories/Education/college-fee-icon.png")
                    }
                }
            };
        }
    }