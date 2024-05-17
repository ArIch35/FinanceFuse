using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Models;
using FinanceFuse.Services;
using FinanceFuse.ViewModels.TransactionsViewModel;

namespace FinanceFuse.Interfaces;

public interface IRoutable
{
    public void OnRouted<TItem, TScreen>(TItem? item = default, TScreen? currentRef = default) 
        where TItem : IModelBase
        where TScreen : ObservableObject, IRoutable;
}