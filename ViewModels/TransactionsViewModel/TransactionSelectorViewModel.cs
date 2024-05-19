using System;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Services;
using System.Collections.Generic;
using System.Linq;
using FinanceFuse.Interfaces;
using FinanceFuse.Models;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class SelectorItem(int year, TransactionPageViewModel model)
    {
        public int YearHeader { get; } = year;
        public TransactionPageViewModel TransactionPageModel { get; } = model;
    }
    public partial class TransactionSelectorViewModel: RoutableObservableBase
    {
        public static List<SelectorItem> GroupedItemsByYear => GenerateGroupedItemsByYear();
        [ObservableProperty] private TransactionPageViewModel _transactionPageView = null!;
        [ObservableProperty] private double _totalYearValue;
        [ObservableProperty] private string _totalYearDesc = null!;
        [ObservableProperty] private SelectorItem _selectedItem;
        partial void OnSelectedItemChanged(SelectorItem value)
        {
            SelectedItemUpdated();
        }

        public TransactionSelectorViewModel()
        {
            SelectedItem = GroupedItemsByYear.Find(item => item.YearHeader == DateTime.Now.Year) ?? GroupedItemsByYear[0];
            SelectedItemUpdated();
            RoutingService.AddScreenToStaticScreen("TransactionSelectorViewModel", this);
        }

        private void SelectedItemUpdated()
        {
            TotalYearDesc = SelectedItem.YearHeader.ToString();
            TotalYearValue = TransactionService.GetTransactionSumOfYear(SelectedItem.YearHeader);
            TransactionPageView = SelectedItem.TransactionPageModel;
        }
        
        private static List<SelectorItem> GenerateGroupedItemsByYear()
        {
            return [.. TransactionService.GetTransactionsByMonthAndYear()
                .Select(outerPair => new SelectorItem(outerPair.Key, 
                    new TransactionPageViewModel(outerPair.Value.Select(innerPair => 
                        new TabItem(innerPair.Key, 
                            new TransactionItemViewModel(innerPair.Value.Select(value => 
                                new TransactionItem(value))
                                .OrderBy(item => item.Transaction.Date).ToList()))).ToList())))
                  .OrderBy(item => item.YearHeader)];
        }

        public override void OnRouted(IModelBase? item = default, RoutableObservableBase? currentRef = default) 
        {
            if (!(item is Transaction transaction))
            {
                return;
            }
            SelectedItem = GroupedItemsByYear.First(selected => selected.YearHeader == transaction.Date.Year);
            SelectedItem.TransactionPageModel.ChangeTab(transaction.Date.Month);
        }
        
        public void AddNewTransaction()
        {
            RoutingService.ChangeScreen(new TransactionDetailsViewModel(
                new Transaction()
                {
                    Date = DateTime.Now,
                    Category = CategoryService.NoCategory
                }));
        }
    }
}
