
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * PercentageConverter file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 11-Sep-17 7:08:19 PM
* ******************************************************************************/

using System;
using System.Windows.Data;

namespace VollomeStudio.Helpers
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToDouble(value) *
                   System.Convert.ToDouble(parameter);
        }

        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}