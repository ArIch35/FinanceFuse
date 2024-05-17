using FinanceFuse.Models;

namespace FinanceFuse.Interfaces;
public interface IRoutable
{
    public void OnRouted(IModelBase? item = default, RoutableObservableBase? currentRef = default);
}