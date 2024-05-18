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
        [ObservableProperty] private ObservableObject _mainContentViewModel = null!;
        public List<SelectorItem> GroupedItemsByYear => GenerateGroupedItemsByYear();
        
        private SelectorItem _selectedItem;
        public SelectorItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                SetProperty(ref _selectedItem, value);
                SelectedItemUpdated();
            }
        }

        private double _totalYearValue;
        public double TotalYearValue
        {
            get => _totalYearValue;
            set => SetProperty(ref _totalYearValue, value);
        }
        
        private string _totalYearDesc = null!;
        public string TotalYearDesc
        {
            get => _totalYearDesc;
            set => SetProperty(ref _totalYearDesc, $"Balance {value}");
        }

        public TransactionSelectorViewModel()
        {
            SelectedItem = _selectedItem = GroupedItemsByYear.FirstOrDefault()!;
            SelectedItemUpdated();
            RoutingService.AddScreenToStaticScreen("TransactionSelectorViewModel", this);
        }

        private void SelectedItemUpdated()
        {
            TotalYearDesc = SelectedItem.YearHeader.ToString();
            TotalYearValue = TransactionService.GetTransactionSumOfYear(SelectedItem.YearHeader);
            MainContentViewModel = SelectedItem.TransactionPageModel;
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
    }
}
