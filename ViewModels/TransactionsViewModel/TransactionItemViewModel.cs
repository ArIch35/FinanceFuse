using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Models;
using FinanceFuse.Services;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class TransactionItem(Transaction transaction): ObservableObject
    {
        public Transaction Transaction { get; } = transaction;
        public void OnTransactionClicked()
        {
            RoutingService.ChangeScreen(new TransactionDetailsViewModel(Transaction));
        }
    }
    public class TransactionItemViewModel(List<TransactionItem> transactions) : ObservableObject
    {
        public List<TransactionItem> Transactions { get; } = transactions;
    }
}

