﻿using System.Globalization;
using System.Windows.Data;

namespace DissertationThemes.ViewerApp.Components;

public class BoolTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Yes" : "No";
        }
        return "No";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString().ToLower() == "yes";
    }
}
