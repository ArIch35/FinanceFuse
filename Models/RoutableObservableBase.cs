using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Interfaces;

namespace FinanceFuse.Models;

public abstract class RoutableObservableBase: ObservableObject, IRoutable
{
    public abstract void OnRouted(IModelBase? item = default, RoutableObservableBase? currentRef = default);
}