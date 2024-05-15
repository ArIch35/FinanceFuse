using FinanceFuse.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceFuse.Services
{
    public class TransactionService
    {
        private List<Transaction> Transactions { get; }
        private static TransactionService _instance = null!;
        private static readonly  object Locker = new();

        private TransactionService()
        {
            Transactions = InitRandomData();
        }
        public static TransactionService GetInstance()
        {
            if (_instance != null)
                return _instance;

            lock (Locker)
            {
                _instance ??= new TransactionService();
            }

            return _instance;
        }

        private static List<Transaction> InitRandomData()
        {
            return Enumerable.Range(1, 72).Select(o => new Transaction()
            {
                Id = $"{o}",
                ImageUrl = "Assets/avalonia-logo.ico",
                Description = $"Details {o}",
                Date = new DateTime(2000 + o%5, 1 + o%12, o < 25 ? o : o % 25 + 1),
                Price = 1.3445 + o,
            }).ToList();
        }

        public SortedDictionary<int, SortedDictionary<DateTime, List<Transaction>>> GetTransactionsByMonthAndYear()
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
    }
}
