
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * CountConverter file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 10-Nov-17 4:55:47 PM
* ******************************************************************************/

using System;
using System.Globalization;
using System.Windows.Data;

namespace VollomeStudio.Helpers
{
    public class CountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                string[] units = { "", "K", "M", "B" };
                long size = System.Convert.ToInt64(value);
                int unit = 0;
                while (size >= 1000)
                {
                    size /= 1000;
                    ++unit;
                }
                return String.Format("{0:0.#} {1}", size, units[unit]);
            }
            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}