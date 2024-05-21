using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Interfaces;
using FinanceFuse.Models;
using FinanceFuse.Services;
using FinanceFuse.ViewModels.CategoriesViewModel;
using FinanceFuse.ViewModels.HomePagesViewModel;
using NCalc;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public partial class TransactionDetailsViewModel: RoutableObservableBase
    {
        private Transaction Transaction { get; }
        
        [ObservableProperty] private bool _isFormValid;
        [ObservableProperty] private string _description = null!;
        partial void OnDescriptionChanged(string value)
        {
            Transaction.Description = value;
            IsFormValid = Validate(Transaction);
        }
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
        
        [ObservableProperty] private DateTimeOffset _date;
        partial void OnDateChanged(DateTimeOffset value)
        {
            Transaction.Date = value.DateTime;
            IsFormValid = Validate(Transaction);
        }
        
        [ObservableProperty] private Category _category = null!;
        partial void OnCategoryChanged(Category value)
        {
            Transaction.Category = value;
            IsFormValid = Validate(Transaction);
        }

        private string _prevStaticScreenRefId = "TransactionSelectorViewModel";

        public TransactionDetailsViewModel(Transaction transaction)
        {
            Transaction = transaction.Copy();
            Description = transaction.Description;
            PriceString = transaction.Price.ToString(CultureInfo.CurrentCulture);
            Date = new DateTimeOffset(Transaction.Date);
            Category = transaction.Category;
        }

        public void BackToHome()
        {
            RoutingService.ChangeStaticScreen(_prevStaticScreenRefId, Transaction);
        }
        
        public void GoToCategory()
        {
            RoutingService.ChangeStaticScreen(nameof(CategoryPageViewModel), this);
        }

        public void ParseExpression()
        {
            try
            {
                var expression = new Expression(PriceString);
                PriceString = expression.Evaluate().ToString()!;
                Transaction.Price = double.Parse(PriceString);
                IsFormValid = Validate(Transaction);
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
            if (item is Transaction transaction)
            {
                if (transaction.Id != null!)
                {
                    IsFormValid = true;
                }
            }
            
            if (item is Category category)
            {
                Category = category;
            }

            if (currentRef != null)
            {
                _prevStaticScreenRefId = currentRef is HomePageViewModel ? nameof(HomePageViewModel) : nameof(TransactionSelectorViewModel);
            }
        }

        public void SaveChanges()
        {
            var currentTransaction = TransactionService.GetTransactionById(Transaction.Id);
            
            if (currentTransaction == null)
            {
                TransactionService.AddNewTransaction(Transaction);
                return;
            }
            
            if (!currentTransaction.IsValueEqual(Transaction))
            {
                currentTransaction.Update(Transaction);
            }
        }

        private static bool Validate(Transaction transaction)
        {
            return transaction.Description?.Trim().Length > 0 &&
                   transaction.Price > 0 &&
                   !transaction.Category.Id.Equals("0");
        }
    }
}
