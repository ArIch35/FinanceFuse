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
        var categories = new List<Category>
        {
            new Category(
                id: "1",
                type: CategoryType.Expense,
                name: "Bills & Utilities",
                logoUrl: "Assets/Categories/Bills/bills-icon.png"
            )
            {
                SubCategories = new List<Category>
                {
                    new Category(
                        id: "2",
                        type: CategoryType.Expense,
                        name: "Electric Bills",
                        logoUrl: "Assets/Categories/Bills/electric-bills-icon.png"
                    ),
                    new Category(
                        id: "3",
                        type: CategoryType.Expense,
                        name: "Gas Bills",
                        logoUrl: "Assets/Categories/Bills/gas-bills-icon.png"
                    ),
                    new Category(
                        id: "4",
                        type: CategoryType.Expense,
                        name: "House Bills",
                        logoUrl: "Assets/Categories/Bills/house-bills-icon.png"
                    ),
                    new Category(
                        id: "5",
                        type: CategoryType.Expense,
                        name: "Internet Bills",
                        logoUrl: "Assets/Categories/Bills/internet-bills-icon.png"
                    ),
                    new Category(
                        id: "6",
                        type: CategoryType.Expense,
                        name: "Phone Bills",
                        logoUrl: "Assets/Categories/Bills/phone-bills-icon.png"
                    ),
                    new Category(
                        id: "7",
                        type: CategoryType.Expense,
                        name: "Water Bills",
                        logoUrl: "Assets/Categories/Bills/water-bills-icon.png"
                    )
                }
            },
            new Category(
                id: "8",
                type: CategoryType.Expense,
                name: "Education",
                logoUrl: "Assets/Categories/Education/education-icon.png"
            )
            {
                SubCategories = new List<Category>
                {
                    new Category(
                        id: "9",
                        type: CategoryType.Expense,
                        name: "Books",
                        logoUrl: "Assets/Categories/Education/books-bills-icon.png"
                    ),
                    new Category(
                        id: "10",
                        type: CategoryType.Expense,
                        name: "College Fee",
                        logoUrl: "Assets/Categories/Education/college-fee-icon.png"
                    )
                }
            },
            new Category(
                id: "11",
                type: CategoryType.Expense,
                name: "Entertainment",
                logoUrl: "Assets/Categories/Entertainment/entertainment-icon.png"
            )
            {
                SubCategories = new List<Category>
                {
                    new Category(
                        id: "12",
                        type: CategoryType.Expense,
                        name: "Games",
                        logoUrl: "Assets/Categories/Entertainment/game-bills-icon.png"
                    ),
                    new Category(
                        id: "13",
                        type: CategoryType.Expense,
                        name: "Shopping",
                        logoUrl: "Assets/Categories/Entertainment/shopping-icon.png"
                    ),
                    new Category(
                        id: "14",
                        type: CategoryType.Expense,
                        name: "Streaming",
                        logoUrl: "Assets/Categories/Entertainment/streaming-bills-icon.png"
                    )
                }
            },
            new Category(
                id: "15",
                type: CategoryType.Expense,
                name: "Food",
                logoUrl: "Assets/Categories/Food/food-icons.png"
            ),
            new Category(
                id: "16",
                type: CategoryType.Expense,
                name: "Insurance",
                logoUrl: "Assets/Categories/Insurance/insurance-icon.png"
            ),
            new Category(
                id: "17",
                type: CategoryType.Expense,
                name: "Transportation",
                logoUrl: "Assets/Categories/Transportation/transportation-icon.png"
            ),
            new Category(
                id: "18",
                type: CategoryType.Expense,
                name: "Other Expenses",
                logoUrl: "Assets/Categories/other-expenses-icon.png"
            ),
            new Category(
                id: "19",
                type: CategoryType.Income,
                name: "Salary",
                logoUrl: "Assets/Categories/Income/salary-icon.png"
            ),
        };

        return categories;
    }
}