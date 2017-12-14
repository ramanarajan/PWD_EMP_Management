
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * BooleanToInvertVisibilityConverter file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 11-Sep-17 4:59:28 PM
* ******************************************************************************/

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VollomeStudio.Helpers
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BooleanToInvertVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public BooleanToInvertVisibilityConverter()
        {
            // set defaults
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return null;
            return (bool)value ?  FalseValue : TrueValue;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (Equals(value, TrueValue))
                return true;
            if (Equals(value, FalseValue))
                return false;
            return null;
        }
    }

}