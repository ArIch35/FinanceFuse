﻿using System;
using CommunityToolkit.Mvvm.ComponentModel;
using FinanceFuse.Services;
using System.Collections.Generic;
using System.Linq;
using FinanceFuse.Interfaces;
using FinanceFuse.Models;

namespace FinanceFuse.ViewModels.TransactionsViewModel
{
    public class SelectorItem(int year, TransactionPageViewModel model)
    {
        public int YearHeader { get; } = year;
        public TransactionPageViewModel TransactionPageModel { get; } = model;
    }
    public partial class TransactionSelectorViewModel: RoutableObservableBase
    {
        [ObservableProperty] private bool _hasItems;
        public List<SelectorItem> GroupedItemsByYear { get; set; }
        [ObservableProperty] private TransactionPageViewModel _transactionPageView = null!;
        [ObservableProperty] private double _totalYearValue;
        [ObservableProperty] private string _totalYearDesc = null!;
        [ObservableProperty] private SelectorItem _selectedItem = null!;
        partial void OnSelectedItemChanged(SelectorItem value)
        {
            if (value != null!)
            {
                TotalYearDesc = value.YearHeader.ToString();
                TotalYearValue = TransactionService.GetTransactionSumOfYear(value.YearHeader);
                TransactionPageView = value.TransactionPageModel;
            }
        }

        public TransactionSelectorViewModel()
        {
            GroupedItemsByYear = GenerateGroupedItemsByYear();
            HasItems = GroupedItemsByYear.Count > 0;
            if (!HasItems)
            {
                return;
            }
            SelectedItem = GroupedItemsByYear.Find(item => item.YearHeader == DateTime.Now.Year) ?? GroupedItemsByYear[0];
        }
        
        private static List<SelectorItem> GenerateGroupedItemsByYear()
        {
            return [.. TransactionService.GetTransactionsByMonthAndYear()
                .Select(outerPair => new SelectorItem(outerPair.Key, 
                    new TransactionPageViewModel(outerPair.Value.Select(innerPair => 
                        new TabItem(innerPair.Key, 
                            new TransactionItemViewModel(
                                innerPair.Value.GroupBy(value => value.Category.Id)
                                    .Select(group => new TransactionItem(
                                        new Transaction()
                                        {
                                            Id = "-1",
                                            Category = group.First().Category,
                                            Description = group.First().Category.Name,
                                            Date = group.OrderByDescending(trans => trans.Date).First().Date,
                                            Price = group.Aggregate(0.0, (acc, transaction) => acc += transaction.Price)
                                        }, group.Select(transaction => new TransactionItem(transaction)).ToList()))
                                        .OrderByDescending(item => item.Transaction.Date).ToList()))).ToList())))
                  .OrderBy(item => item.YearHeader)];
        }

        public override void OnRouted(IModelBase? item = default, RoutableObservableBase? currentRef = default) 
        {
            if (!(item is Transaction transaction))
            {
                return;
            }

            GroupedItemsByYear = GenerateGroupedItemsByYear();
            HasItems = GroupedItemsByYear.Count > 0;
            SelectedItem = GroupedItemsByYear.First(selected => selected.YearHeader == transaction.Date.Year);
            SelectedItem?.TransactionPageModel.ChangeTab(transaction.Date.Month);
        }
        
        public void AddNewTransaction()
        {
            RoutingService.ChangeScreen(new TransactionDetailsViewModel(
                new Transaction()
                {
                    Date = DateTime.Now,
                    Category = CategoryService.NoCategory
                }));
        }
    }
}
