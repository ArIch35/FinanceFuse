using Avalonia.Media.Imaging;
using FinanceFuse.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFuse.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public Bitmap ImageSource
        {
            get => BitmapReader.ReadBitmapFromStringURI(ImageUrl);
        }

        public Transaction()
        {
            Id = "";
            ImageUrl = "";
            Description = "";
        }
    }
}
