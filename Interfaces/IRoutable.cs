using FinanceFuse.Models;
using FinanceFuse.Services;
using FinanceFuse.ViewModels.TransactionsViewModel;

namespace FinanceFuse.Interfaces;

public interface IRoutable
{
    public void OnRouted<T>(T item) where T : IModelBase;
}