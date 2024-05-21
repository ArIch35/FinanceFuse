using System.Linq;
using FinanceFuse.Interfaces;
using FinanceFuse.Models;
using FinanceFuse.Services;
using FinanceFuse.ViewModels.TransactionsViewModel;

namespace FinanceFuse.ViewModels;

public class HomePageViewModel: RoutableObservableBase
{
    public TransactionItemViewModel RecentTransactions { get; }

    public HomePageViewModel()
    {
        RecentTransactions = new(TransactionService.GetRecentTransactions(), this);
    }

    public void ToTransactionSelector()
    {
        RoutingService.ChangeStaticScreen(nameof(TransactionSelectorViewModel));
    }

    public override void OnRouted(IModelBase? item = default, RoutableObservableBase? currentRef = default)
    {
        RecentTransactions.Transactions = TransactionService.GetRecentTransactions()
            .Select(transaction => new TransactionItem(transaction, this));
    }
}