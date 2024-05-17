using System.Collections.Generic;
using System.Linq;
using FinanceFuse.Interfaces;
using FinanceFuse.Models;
using FinanceFuse.Services;
using FinanceFuse.ViewModels.TransactionsViewModel;

namespace FinanceFuse.ViewModels.CategoriesViewModel;

public class CategoryPageViewModel: RoutableObservableBase
{
    private static readonly CategoryService CategoryService = CategoryService.GetInstance();
    private RoutableObservableBase _senderTransactionDetailRef = null!;
    public List<CategoryItemViewModel> CategoryItems { get; }

    public CategoryPageViewModel()
    {
        CategoryItems = GetCategoryItemViewModels(CategoryService);
        RoutingService.GetInstance().AddScreenToStaticScreen("CategoryPageViewModel", this);
    }

    private List<CategoryItemViewModel> GetCategoryItemViewModels(CategoryService service)
    {
        return service.Categories.Select(category => new CategoryItemViewModel(category, OnCategoryClicked)).ToList();
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