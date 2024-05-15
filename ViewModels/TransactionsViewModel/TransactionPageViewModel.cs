using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class TabItem(DateTime header, TransactionItemViewModel model)
    {
        public DateTime Header { get; set; } = header;
        public TransactionItemViewModel TransactionModel { get; set; } = model;
    }
    public class TransactionPageViewModel(List<TabItem> items) : ObservableObject
    {
        public List<TabItem> TabItems { get; } = items;
        
        private TabItem _selectedItem = null!;
        public TabItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        
        public void ChangeTab(int month)
        {
            SelectedItem = TabItems.First(item => item.Header.Date.Month == month);
        }
    }
}
