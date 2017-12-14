
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * BooleanIndirectConverter file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 25-Sep-17 4:42:55 PM
* ******************************************************************************/

using System;
using System.Globalization;
using System.Windows.Data;

namespace VollomeStudio.Helpers
{
    public class BooleanIndirectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return !(bool)value;
            if (value is int)
                return (int)value == 0;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}