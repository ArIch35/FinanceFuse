using System;
using System.Globalization;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Models;
using FinanceFuse.Services;
using NCalc;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class TransactionDetailsViewModel: ObservableObject
    {
        public Transaction Transaction { get; }
        private static readonly RoutingService Router = RoutingService.GetInstance();
        
        private string _priceString = "";
        public string PriceString
        {
            get => _priceString;
            set => SetProperty(ref _priceString, value);
        }

        private DateTimeOffset _date;
        public DateTimeOffset Date
        {
            get => _date;
            set
            {
                Transaction.Date = value.DateTime;
                SetProperty(ref _date, value);
            }
        }

        public TransactionDetailsViewModel(Transaction transaction)
        {
            Transaction = transaction;
            PriceString = transaction.Price.ToString(CultureInfo.CurrentCulture);
            Date = new DateTimeOffset(Transaction.Date);
        }

        public void BackToHome()
        {
            Router.ChangeStaticScreen("TransactionSelectorViewModel", Transaction);
        }

        public void ParseExpression()
        {
            try
            {
                var expression = new Expression(PriceString);
                PriceString = expression.Evaluate().ToString()!;
                Transaction.Price = double.Parse(PriceString);
            }
            catch (Exception ex) when (ex is EvaluationException || ex is ArgumentException)
            {
                PriceString = CorrectExpression(PriceString);
            }
        }

        private static string CorrectExpression(string expression)
        {
            return string.Concat(expression.Where(s => s >= 42 && s <= 57));
        }
    }
}
