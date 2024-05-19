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
    
    public partial class TransactionPageViewModel: ObservableObject
    {
        public List<TabItem> TabItems { get; }
        [ObservableProperty] private TabItem _selectedTab = null!;

        public TransactionPageViewModel(List<TabItem> tabItems)
        {
            TabItems = tabItems;
            SelectedTab = TabItems.Find(tab => tab.Header.Month == DateTime.Now.Month) ?? TabItems[0];
        }
        
        public void ChangeTab(int month)
        {
            SelectedTab = TabItems.First(item => item.Header.Date.Month == month);
        }
    }
}
