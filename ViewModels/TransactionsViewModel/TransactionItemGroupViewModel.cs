using System.Collections.Generic;
using System.Linq;
using FinanceFuse.Models;
using FinanceFuse.Services;

namespace FinanceFuse.ViewModels.TransactionsViewModel;

public class TransactionItemGroup(Transaction transaction, IEnumerable<Transaction> groupedTransaction)
{
    public Transaction Transaction { get; } = transaction;
    public string GroupCount => $"{groupedTransaction.Count()} Transactions";
    public TransactionItemViewModel TransactionItemView { get; } = new(groupedTransaction);
}
public class TransactionItemGroupViewModel
{
    public IEnumerable<TransactionItemGroup> Transactions { get; }
    public double Income { get; }
    public double Expenses { get; } 
    public double Total { get; }
    public string TotalColor { get; } = null!;
        
    public TransactionItemGroupViewModel(List<IGrouping<string, Transaction>> groupedTransactions)
    {
        Transactions = groupedTransactions.Select(group => new TransactionItemGroup(
            new Transaction()
            {
                Id = "-1",
                Category = group.First().Category,
                Description = group.First().Category.Name,
                Date = group.OrderByDescending(transaction => transaction.Date).First().Date,
                Price = group.Sum(transaction => transaction.Price)
            }, group)).ToList();
        
        if (!Transactions.Any())
        {
            return;
        }

        var firstTransactionDate = Transactions.First().Transaction.Date;
        Income = TransactionService.GetTransactionSumOfMonth(firstTransactionDate, CategoryType.Income);
        Expenses = TransactionService.GetTransactionSumOfMonth(firstTransactionDate, CategoryType.Expense);
        Total = Income - Expenses;
        TotalColor = Total < 0 ? "#d74045" : "#ffffff";
    }

}