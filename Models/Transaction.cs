using Avalonia.Media.Imaging;
using FinanceFuse.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceFuse.Interfaces;

namespace FinanceFuse.Models
{
    public class Transaction: IModelBase
    {
        public string Id { get; init; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public Bitmap ImageSource => BitmapReader.ReadBitmapFromStringUri(ImageUrl);
        private Category _category = null!;

        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                ImageUrl = value.LogoUrl!;
            }
        }
    }
}
