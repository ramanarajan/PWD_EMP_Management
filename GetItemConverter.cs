
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * GetItemConverter file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 22-Nov-17 1:15:34 PM
* ******************************************************************************/

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VollomeStudio.Helpers
{
    public class GetItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count = -1;
            if (parameter != null)
                count = (int)parameter;
            if (value != null)
            {
                System.Collections.IList _il = value as System.Collections.IList;
                if (_il != null)
                {
                    if (count == -1)
                        count = _il.Count - 1;
                    if (count < _il.Count && count>-1)
                        return _il[count];
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}