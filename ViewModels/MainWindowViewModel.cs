using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Services;
using FinanceFuse.ViewModels.TransactionsViewModel;

namespace FinanceFuse.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private ObservableObject _mainContentViewModel = new TransactionSelectorViewModel();
    public MainWindowViewModel()
    {
        RoutingService.SetMainCallback(ChangeScreen);
    }
    
    private void ChangeScreen(ObservableObject newScreen)
    {
        MainContentViewModel = newScreen;
    }
}