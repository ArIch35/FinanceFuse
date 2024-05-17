using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Models;

namespace FinanceFuse.ViewModels.CategoriesViewModel;

public class CategoryItem(Category category, Action<Category> callback)
{
    public Category Category { get; } = category;
    private Action<Category> CategoryChosenCallback { get; } = callback;

    public void CategoryClicked()
    {
        CategoryChosenCallback(Category);
    }
}
public class CategoryItemViewModel: ObservableObject
{
    public CategoryItem MainCategory { get; }
    public List<CategoryItem> SubCategories { get; }
    private static Action<Category> _categoryChosenCallback = null!;

    public CategoryItemViewModel(Category category, Action<Category> callback)
    {
        MainCategory = new CategoryItem(category, OnCategoryClicked);
        SubCategories = category.SubCategories.Select(cat => new CategoryItem(cat, OnCategoryClicked)).ToList();
        _categoryChosenCallback = callback;
    }
    private static void OnCategoryClicked(Category category)
    {
        _categoryChosenCallback(category);
    }
}