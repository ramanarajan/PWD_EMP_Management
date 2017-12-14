
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * FilesizeConverter file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 12-Dec-17 5:48:02 PM
* ******************************************************************************/

using System;
using System.Globalization;
using System.Windows.Data;

namespace VollomeStudio.Helpers
{
    public class FilesizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                const int byteConversion = 1024;
                double bytes = System.Convert.ToDouble(value);

                if (bytes >= Math.Pow(byteConversion, 3)) //GB Range
                    return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 3), 2), " GB");
                else if (bytes >= Math.Pow(byteConversion, 2)) //MB Range
                    return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 2), 2), " MB");
                else if (bytes >= byteConversion) //KB Range
                    return string.Concat(Math.Round(bytes / byteConversion, 2), " KB");
                else //Bytes
                    return string.Concat(bytes, " Bytes");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}