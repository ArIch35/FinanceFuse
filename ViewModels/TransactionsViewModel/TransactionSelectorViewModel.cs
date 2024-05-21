using System;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Services;
using System.Collections.Generic;
using System.Linq;
using FinanceFuse.Interfaces;
using FinanceFuse.Models;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class SelectorItem(int year, SortedDictionary<DateTime, List<Transaction>> dictionary)
    {
        public int YearHeader { get; } = year;
        public TransactionPageViewModel TransactionPageModel { get; } = new(dictionary);
    }
    
    public partial class TransactionSelectorViewModel: RoutableObservableBase
    {
        [ObservableProperty] private bool _hasItems;
        [ObservableProperty] private TransactionPageViewModel _transactionPageView = null!;
        [ObservableProperty] private double _totalYearValue;
        [ObservableProperty] private string _totalYearDesc = null!;
        [ObservableProperty] private SelectorItem? _selectedItem;
        public IEnumerable<SelectorItem> GroupedItemsByYear { get; set; }

        partial void OnSelectedItemChanged(SelectorItem? value)
        {
            if (value == null!)
            {
                return;
            }
            TotalYearDesc = value.YearHeader.ToString();
            TotalYearValue = TransactionService.GetTransactionSumOfYear(value.YearHeader);
            TransactionPageView = value.TransactionPageModel;
        }

        public TransactionSelectorViewModel()
        {
            GroupedItemsByYear = GenerateGroupedItemsByYear();
            HasItems = GroupedItemsByYear.Any();
            if (!HasItems)
            {
                return;
            }
            SelectedItem = GroupedItemsByYear.FirstOrDefault(item => item.YearHeader == DateTime.Now.Year, GroupedItemsByYear.First());
        }
        
        private static List<SelectorItem> GenerateGroupedItemsByYear()
        {
            return
            [
                .. TransactionService.GetTransactionsByMonthAndYear()
                    .Select(transaction => new SelectorItem(transaction.Key, transaction.Value))
            ];
        }

        public override void OnRouted(IModelBase? item = default, RoutableObservableBase? currentRef = default) 
        {
            if (!(item is Transaction transaction))
            {
                return;
            }

            GroupedItemsByYear = GenerateGroupedItemsByYear();
            HasItems = GroupedItemsByYear.Any();
            SelectedItem = GroupedItemsByYear.First(selected => selected.YearHeader == transaction.Date.Year);
            SelectedItem?.TransactionPageModel.ChangeTab(transaction.Date.Month);
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

        public void GoToHomeScreen()
        {
            RoutingService.ChangeStaticScreen(nameof(HomePageViewModel));
        }
    }
}
