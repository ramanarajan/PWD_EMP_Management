
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * NativeMethods file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 13-Nov-17 6:01:18 PM
* ******************************************************************************/

using System;
using System.Runtime.InteropServices;


namespace VollomeStudio.Helpers
{
    public static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    }
}