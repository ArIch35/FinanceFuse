using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Models;
using FinanceFuse.Services;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class TransactionItem(Transaction transaction): ObservableObject
    {
        public Transaction Transaction { get; set; } = transaction;
        public void OnTransactionClicked()
        {
            
        }
    }
    public class TransactionItemViewModel(List<TransactionItem> transactions) : ObservableObject
    {
        public List<TransactionItem> Transactions { get; } = transactions;
    }
}

