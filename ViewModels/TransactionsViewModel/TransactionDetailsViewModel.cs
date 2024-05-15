using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Models;
using FinanceFuse.Services;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class TransactionDetailsViewModel(Transaction transaction): ObservableObject
    {
        public Transaction Transaction { get; } = transaction;
        private static readonly RoutingService Router = RoutingService.GetInstance();

        public void BackToHome()
        {
            Router.ChangeScreen("TransactionSelectorViewModel");
        }
    }
}
