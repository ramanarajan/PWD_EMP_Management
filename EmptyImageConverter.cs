/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * EmptyImageConverter file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 03-Oct-17 3:25:21 PM
* ******************************************************************************/



using Microsoft.Win32.SafeHandles;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VollomeStudio.Helpers
{
    public class EmptyImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BitmapImage)
                return value;

            string filePath = value as string;

            try
            {
                if (value == null || ((value as string) ?? "").Length == 0)
                {
                    if (parameter != null)
                    {
                        if (value != null)
                            return string.Format(parameter as string, value);
                        return parameter as string;
                    }

                    return (ImageSource)null;
                }
                if (File.Exists(value as string))
                {
                    if (ContentProviderCore.Helpers.NativeMethods.PathIsUNC(value as string))
                    {
                        BitmapImage bitmapimage = new BitmapImage();
                        bitmapimage.BeginInit();
                        bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapimage.StreamSource = GetMemorystream(value as string);
                        bitmapimage.EndInit();
                        if (bitmapimage.CanFreeze)
                            bitmapimage.Freeze();
                        return bitmapimage;
                    }
                }
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public MemoryStream GetMemorystream(string _filename)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                FileStream f = null;
                SafeFileHandle readSafeFileHandle = ContentProviderCore.Helpers.Utils.CerateSafeFileHandler(_filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                f = new FileStream(readSafeFileHandle, FileAccess.Read);
                ms = new MemoryStream();
                f.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                f.Close();
            }
            catch { }
            return ms;

        }
    }

}