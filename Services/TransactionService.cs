using FinanceFuse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using FinanceFuse.ViewModels.HomePagesViewModel;

namespace FinanceFuse.Services
{
    public static class TransactionService
    {
        private static List<Transaction> Transactions { get; }
        private static int _ctr;

        static TransactionService()
        {
            Transactions = InitRandomData();
        }

        private static List<Transaction> InitRandomData()
        {
            //return [];
            return Enumerable.Range(1, 72).Select(o => new Transaction()
            {
                Id = $"{o}",
                Description = $"Details {o}",
                Date = new DateTime(2024 + o%2, 1 + o%12, o < 25 ? o : o % 25 + 1),
                Price = 1.3445 + o,
                Category = CategoryService.Categories[o % CategoryService.Categories.Count]
            }).ToList();
        }

        public static SortedDictionary<int, SortedDictionary<DateTime, List<Transaction>>> GetTransactionsByMonthAndYear()
        {
            return Transactions.Aggregate(new SortedDictionary<int, SortedDictionary<DateTime, List<Transaction>>>(), (acc, value) =>
            {
                var currentYear = value.Date.Year;
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
                .Sum(transaction => transaction.Price);
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

        public static Transaction? GetTransactionById(string id)
        {
            return Transactions.Find(transaction => transaction.Id == id);
        }

        public static void AddNewTransaction(Transaction transaction)
        {
            transaction.Id = GenerateRandomId();
            Transactions.Add(transaction);
        }

        private static string GenerateRandomId()
        {
            _ctr += 1;
            return $"id-{_ctr}";
        }

        public static IEnumerable<Transaction> GetRecentTransactions()
        {
            return Transactions.Where(transaction => transaction.Date <= DateTime.Now)
                .OrderByDescending(transaction => transaction.Date).Take(3);
        }

        public static IEnumerable<CategorySum> GetTopSpendingForThisMonth()
        {
            var categorySums = Transactions.Where(transaction =>
                    transaction.Date.Year == DateTime.Now.Year && transaction.Date.Month == DateTime.Now.Month)
                .GroupBy(transaction => transaction.Category.Id)
                .Select(group => new CategorySum(group.First().Category, group.Sum(transaction => transaction.Price))).ToList();
            var totalPrice = categorySums.Sum(categorySum => categorySum.Total);
            categorySums.ForEach(categorySum => categorySum.Percentage = Convert.ToUInt16(categorySum.Total/totalPrice*100));
            return categorySums.OrderByDescending(categorySum => categorySum.Total).Take(3);
        }
    }
}
