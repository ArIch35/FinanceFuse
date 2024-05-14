using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
