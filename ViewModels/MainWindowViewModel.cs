using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Services;
using FinanceFuse.ViewModels.CategoriesViewModel;
using FinanceFuse.ViewModels.TransactionsViewModel;
using FinanceFuse.ViewModels.HomePagesViewModel;

namespace FinanceFuse.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private ObservableObject _mainContentViewModel = new HomePageViewModel();
    public MainWindowViewModel()
    {
        RoutingService.SetMainCallback(ChangeScreen);
        InitStaticScreen();
    }
    
    private void ChangeScreen(ObservableObject newScreen)
    {
        MainContentViewModel = newScreen;
    }

    private static void InitStaticScreen()
    {
        RoutingService.AddScreenToStaticScreen(nameof(HomePageViewModel), new HomePageViewModel());
        RoutingService.AddScreenToStaticScreen(nameof(TransactionSelectorViewModel), new TransactionSelectorViewModel());
        RoutingService.AddScreenToStaticScreen(nameof(CategoryPageViewModel), new CategoryPageViewModel());
    }
}