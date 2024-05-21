using System.Linq;
using FinanceFuse.Interfaces;
using FinanceFuse.Models;
using FinanceFuse.Services;
using FinanceFuse.ViewModels.TransactionsViewModel;
namespace FinanceFuse.ViewModels.HomePagesViewModel;

public class HomePageViewModel: RoutableObservableBase
{
    public TransactionItemViewModel RecentTransactions { get; }
    public TopSpendingViewModel TopSpending { get; }

    public HomePageViewModel()
    {
        RecentTransactions = new TransactionItemViewModel(TransactionService.GetRecentTransactions(), this);
        TopSpending = new TopSpendingViewModel(TransactionService.GetTopSpendingForThisMonth());
    }

    public void ToTransactionSelector()
    {
        RoutingService.ChangeStaticScreen(nameof(TransactionSelectorViewModel));
    }

    public override void OnRouted(IModelBase? item = default, RoutableObservableBase? currentRef = default)
    {
        RecentTransactions.Transactions = TransactionService.GetRecentTransactions()
            .Select(transaction => new TransactionItem(transaction, this));
        TopSpending.CategorySums = TransactionService.GetTopSpendingForThisMonth();
    }
}