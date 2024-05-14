using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FinanceFuse.Converter;

public class DateStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            if (parameter is string format)
            {
                return dateTime.ToString(format, culture);
            }
            else
            {
                return dateTime.ToString("D", culture); 
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string dateStr)
        {
            if (DateTime.TryParse(dateStr, out var date))
            {
                return date;
            }
        }

        return null;
    }
}