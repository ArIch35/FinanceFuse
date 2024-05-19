using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Interfaces;
using FinanceFuse.Models;
using FinanceFuse.Services;
using FinanceFuse.ViewModels.TransactionsViewModel;

namespace FinanceFuse.ViewModels.CategoriesViewModel;
public class CategoryTabItems(string header, List<CategoryItemViewModel> categoryItems)
{
    public string Header { get; init; } = header;
    public List<CategoryItemViewModel> CategoryItems { get; init; } = categoryItems;
}

public partial class CategoryPageViewModel: RoutableObservableBase
{
    private RoutableObservableBase _senderTransactionDetailRef = null!;
    public List<CategoryTabItems> CategoryTabItems { get; }
    
    [ObservableProperty] private CategoryTabItems _selectedItem = null!;

    public CategoryPageViewModel()
    {
        CategoryTabItems = GetCategoryItemViewModels();
        RoutingService.AddScreenToStaticScreen("CategoryPageViewModel", this);
    }

    private List<CategoryTabItems> GetCategoryItemViewModels()
    {
        return CategoryService.Categories.GroupBy(category => category.Type)
            .Select(grouped => 
                new CategoryTabItems(grouped.Key.ToString(), grouped.Select(category => 
                    new CategoryItemViewModel(category, OnCategoryClicked)).ToList()))
            .ToList();
    }

    private void OnCategoryClicked(Category category)
    {
        RoutingService.ChangeScreen(_senderTransactionDetailRef, category);
    }

    public override void OnRouted(IModelBase? item = default, RoutableObservableBase? currentRef = default)
    {
        if (currentRef is TransactionDetailsViewModel transactionDetailsViewModel)
        {
            _senderTransactionDetailRef = transactionDetailsViewModel;
        }
    }
}