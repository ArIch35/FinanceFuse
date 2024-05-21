using System;
using FinanceFuse.Interfaces;

namespace FinanceFuse.Models
{
    public class Transaction: IModelBase
    {
        public string Id { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; } = null!;

        public Transaction Copy()
        {
            return new Transaction()
            {
                Id = Id,
                Description = Description,
                Date = Date,
                Price = Price,
                Category = Category
            };
        }

        public bool IsValueEqual(Transaction comparator)
        {
            return Description.Equals(comparator.Description) && Date.Equals(comparator.Date)
                   && Price.Equals(comparator.Price) && Category.IsEqual(comparator.Category);
        }

        public void Update(Transaction newTransaction)
        {
            Description = newTransaction.Description;
            Date = newTransaction.Date;
            Price = newTransaction.Price;
            Category = newTransaction.Category;
        }
    }
}
