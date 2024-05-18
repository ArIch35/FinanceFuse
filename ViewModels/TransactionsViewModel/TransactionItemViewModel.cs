using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Models;
using FinanceFuse.Services;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class TransactionItem(Transaction transaction): ObservableObject
    {
        public Transaction Transaction { get; } = transaction;
        public string Color { get; } = transaction.Category.Type == CategoryType.Expense ? "#d74045" : "#ffffff";
        public void OnTransactionClicked()
        {
            RoutingService.ChangeScreen(new TransactionDetailsViewModel(Transaction));
        }
    }
    public class TransactionItemViewModel: ObservableObject
    {
        public List<TransactionItem> Transactions { get; }
        public double Income { get; }
        public double Expenses { get; } 
        public double Total { get; }
        public string TotalColor { get; } = null!;
        
        public TransactionItemViewModel(List<TransactionItem> transactions)
        {
            Transactions = transactions;
            if (transactions.Count < 0) 
            {
                return;
            }
            Income = TransactionService.GetTransactionSumOfMonth(transactions[0].Transaction.Date, CategoryType.Income);
            Expenses = TransactionService.GetTransactionSumOfMonth(transactions[0].Transaction.Date, CategoryType.Expense);
            Total = Income - Expenses;
            TotalColor = Total < 0 ? "#d74045" : "#ffffff";
        }
    }
}

