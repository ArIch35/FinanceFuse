using FinanceFuse.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceFuse.Services
{
    public static class TransactionService
    {
        private static List<Transaction> Transactions { get; }

        static TransactionService()
        {
            Transactions = InitRandomData();
        }

        private static List<Transaction> InitRandomData()
        {
            return Enumerable.Range(1, 72).Select(o => new Transaction()
            {
                Id = $"{o}",
                ImageUrl = "Assets/avalonia-logo.ico",
                Description = $"Details {o}",
                Date = new DateTime(2023 + o%5, 1 + o%12, o < 25 ? o : o % 25 + 1),
                Price = 1.3445 + o,
                Category = CategoryService.Categories[o % CategoryService.Categories.Count]
            }).ToList();
        }

        public static SortedDictionary<int, SortedDictionary<DateTime, List<Transaction>>> GetTransactionsByMonthAndYear()
        {
            return Transactions.Aggregate(new SortedDictionary<int, SortedDictionary<DateTime, List<Transaction>>>(), (acc, value) =>
            {
                int currentYear = value.Date.Year;
                DateTime currentMonth = new(currentYear, value.Date.Month, 1);
                if (acc.TryGetValue(currentYear, out SortedDictionary<DateTime, List<Transaction>>? transactionsWithMonths))
                {
                    if(transactionsWithMonths.TryGetValue(currentMonth, out List<Transaction>? transactions))
                    {
                        transactions.Add(value);
                    }
                    else
                    {
                       transactionsWithMonths.Add(currentMonth, [value]);
                    }
                }
                else
                {
                    acc.Add(currentYear, new SortedDictionary<DateTime, List<Transaction>>
                    {
                        { currentMonth, [value] }
                    });
                }
                return acc;
            });
        }

        public static double GetTransactionSumOfMonth(DateTime month, CategoryType type)
        {
            return Transactions.Where(transaction =>
                    transaction.Date.Month == month.Month && transaction.Date.Year == month.Year && transaction.Category.Type == type)
                .Aggregate(0.0, (acc, transaction) => acc += transaction.Price);
        }

        public static double GetTransactionSumOfYear(int year)
        {
            return Transactions.Where(transaction => transaction.Date.Year == year).Aggregate(0.0, (acc, transaction) =>
            {
                if (transaction.Category.Type == CategoryType.Expense)
                {
                    acc -= transaction.Price;
                }
                else
                {
                    acc += transaction.Price;
                }
                return acc;
            });
        }
    }
}
