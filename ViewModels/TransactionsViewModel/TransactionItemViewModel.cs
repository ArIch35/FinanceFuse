using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Models;
using FinanceFuse.Services;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class TransactionItem(Transaction transaction, RoutableObservableBase? staticParentRef): ObservableObject
    {
        public Transaction Transaction { get; } = transaction;
        public string Color { get; } = transaction.Category.Type == CategoryType.Expense ? "#d74045" : "#ffffff";
        public void OnTransactionClicked()
        {
            RoutingService.ChangeScreen(new TransactionDetailsViewModel(Transaction), Transaction, staticParentRef);
        }
    }
    public class TransactionItemViewModel(IEnumerable<Transaction> transactions, RoutableObservableBase? staticParentRef = default): ObservableObject
    {
        public IEnumerable<TransactionItem> Transactions { get; set; } = transactions.Select(transaction => new TransactionItem(transaction, staticParentRef));
    }
}

