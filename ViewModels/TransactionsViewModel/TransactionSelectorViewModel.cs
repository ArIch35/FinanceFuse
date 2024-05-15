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
        public int YearHeader { get; set; } = year;
        public TransactionPageViewModel TransactionPageModel { get; set; } = model;
    }
    public partial class TransactionSelectorViewModel: ObservableObject, IRoutable
    {
        [ObservableProperty] private ObservableObject _mainContentViewModel;
        private readonly TransactionService _transactionService = TransactionService.GetInstance();
        private static readonly RoutingService Router = RoutingService.GetInstance();
        public List<SelectorItem> GroupedItemsByYear => GenerateGroupedItemsByYear(_transactionService);
        private SelectorItem _selectedItem;
        public SelectorItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                SetProperty(ref _selectedItem, value);
                MainContentViewModel = _selectedItem.TransactionPageModel;
            }
        }

        public TransactionSelectorViewModel()
        {
            SelectedItem = _selectedItem = GroupedItemsByYear.FirstOrDefault()!;
            MainContentViewModel = SelectedItem.TransactionPageModel;
            Router.AddScreenToStaticScreen("TransactionSelectorViewModel", this);
        }
        private static List<SelectorItem> GenerateGroupedItemsByYear(TransactionService service)
        {
            return [.. service.GetTransactionsByMonthAndYear()
                .Select(outerPair => new SelectorItem(outerPair.Key, 
                    new TransactionPageViewModel(outerPair.Value.Select(innerPair => 
                        new TabItem(innerPair.Key, 
                            new TransactionItemViewModel(innerPair.Value.Select(value => 
                                new TransactionItem(value)).ToList()))).ToList())))
                  .OrderBy(item => item.YearHeader)];
        }
        
        public void OnRouted<T>(T result) where T : IModelBase
        {
            if (result is Transaction transaction)
            {
                SelectedItem = GroupedItemsByYear.First(item => item.YearHeader == transaction.Date.Year);
                SelectedItem.TransactionPageModel.ChangeTab(transaction.Date.Month);
            }
        }
    }
}
