using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FinanceFuse.Converter;

public class PriceConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double price)
        {
            return price.ToString("#.##", culture);
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string priceStr)
        {
            if (double.TryParse(priceStr, out var price))
            {
                return price;
            }
        }

        return null;
    }
}