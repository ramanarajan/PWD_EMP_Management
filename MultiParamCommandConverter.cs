
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * MultiParamCommandConverter file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 5/29/2017 2:09:34 PM
* ******************************************************************************/

using System;
using System.Globalization;
using System.Windows.Data;


namespace VollomeStudio.Helpers
{
    public class MultiParamCommandConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
                return values.Clone();
            return values;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}