/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * RelationalOperatorsConverter file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 27-Sep-17 1:22:14 PM
* ******************************************************************************/

using System;
using System.Globalization;
using System.Windows.Data;

namespace VollomeStudio.Helpers
{
    public class OperatorsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                switch (System.Convert.ToInt32(parameter))
                {
                    case 1: //Less then
                        return System.Convert.ToDouble(values[0]) < System.Convert.ToDouble(values[1]);
                    case 2: //Greater then
                        return System.Convert.ToDouble(values[0]) > System.Convert.ToDouble(values[1]);
                    case 3: //Less then or equal
                        return System.Convert.ToDouble(values[0]) >= System.Convert.ToDouble(values[1]);
                    case 4: //Greater then or equal
                        return System.Convert.ToDouble(values[0]) <= System.Convert.ToDouble(values[1]);
                    case 5: //Equal
                        return System.Convert.ToDouble(values[0]) == System.Convert.ToDouble(values[1]);
                    case 6: //Not equal
                        return System.Convert.ToDouble(values[0]) != System.Convert.ToDouble(values[1]);
                }
            }
            catch { }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}