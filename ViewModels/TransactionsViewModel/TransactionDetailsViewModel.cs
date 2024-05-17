using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using FinanceFuse.Interfaces;
using FinanceFuse.Models;
using FinanceFuse.Services;
using FinanceFuse.ViewModels.CategoriesViewModel;
using NCalc;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class TransactionDetailsViewModel: RoutableObservableBase
    {
        public Transaction Transaction { get; }
        private static readonly RoutingService Router = RoutingService.GetInstance();
        
        private string _priceString = "";
        public string PriceString
        {
            get => _priceString;
            set
            {
                if (value.Any(c => c < 42 || c > 57))
                {
                    return;
                }
                const string pattern = @"[+\*/.,-]{2,}";
                if (Regex.IsMatch(value, pattern))
                {
                    return;
                }
                SetProperty(ref _priceString, value);
            }
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
        
        private Category _category = null!;
        public Category Category
        {
            get => _category;
            private set
            {
                Transaction.Category = value;
                SetProperty(ref _category, value);
            }
        }

        public TransactionDetailsViewModel(Transaction transaction)
        {
            Transaction = transaction;
            PriceString = transaction.Price.ToString(CultureInfo.CurrentCulture);
            Date = new DateTimeOffset(Transaction.Date);
            Category = transaction.Category;
        }

        public void BackToHome()
        {
            Router.ChangeStaticScreen("TransactionSelectorViewModel", Transaction);
        }
        
        public void GoToCategory()
        {
            if (!Router.CheckStaticScreenExist("CategoryPageViewModel"))
            {
                RoutingService.ChangeScreen(new CategoryPageViewModel(), this);
                return;
            }
            Router.ChangeStaticScreen("CategoryPageViewModel", this);
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
                ParseExpression();
            }
        }

        private static string CorrectExpression(string expression)
        {
            var result = string.Concat(expression.Where(s => s >= 42 && s <= 57));
            return char.IsNumber(result[^1]) ? result : result.Substring(0, result.Length - 1);
        }

        public override void OnRouted(IModelBase? item = default, RoutableObservableBase? currentRef = default) 
        {
            if (item is Category category)
            {
                Category = category;
            }
        }
    }
}
