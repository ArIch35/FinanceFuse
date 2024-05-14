using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.ViewModels.TransactionsViewModel;

namespace FinanceFuse.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private ObservableObject _mainContentViewModel = new TransactionSelectorViewModel();
}