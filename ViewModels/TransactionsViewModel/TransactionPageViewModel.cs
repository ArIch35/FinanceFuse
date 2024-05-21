using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using FinanceFuse.Models;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class TabItem(DateTime header, List<IGrouping<string, Transaction>> groupedTransactions)
    {
        public DateTime Header { get; set; } = header;
        public TransactionItemGroupViewModel TransactionItemGroupView { get; set; } = new(groupedTransactions);
    }
    
    public partial class TransactionPageViewModel: ObservableObject
    {
        [ObservableProperty] private TabItem _selectedTab = null!;
        public IEnumerable<TabItem> TabItems { get; }

        public TransactionPageViewModel(SortedDictionary<DateTime, List<Transaction>> transactionsDict)
        {
            TabItems = transactionsDict.Select(transaction => 
                new TabItem(transaction.Key, transaction.Value.GroupBy(grouped => grouped.Category.Id).ToList())).ToList();
            SelectedTab = TabItems.FirstOrDefault(tab => tab.Header.Month == DateTime.Now.Month, TabItems.First());
        }
        
        public void ChangeTab(int month)
        {
            SelectedTab = TabItems.First(item => item.Header.Date.Month == month);
        }
    }
}
