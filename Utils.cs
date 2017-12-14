
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * Utils file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 25-Sep-17 7:29:05 PM
* ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace VollomeStudio.Helpers
{
    internal static class Utils
    {
        public static void RemoveGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        public static void ClearHistory2(Frame _frame)
        {
            if (!_frame.CanGoBack && !_frame.CanGoForward)
                return;
            var entry = _frame.RemoveBackEntry();
            while (entry != null)
                entry = _frame.RemoveBackEntry();
        }


        public static void ClearHistory(Frame _frame)
        {
            if (!_frame.CanGoBack && !_frame.CanGoForward)
                return;
            foreach (JournalEntry journalEntry in _frame.ForwardStack)
                Console.WriteLine(journalEntry.Name);
        }

        public static void Dispose(object parent)
        {
            try
            {
                if (parent == null) return;
                DependencyObject doParent = parent as DependencyObject;
                ReleaseBinding(doParent);
                parent = null;
                doParent = null;
            }
            catch (Exception ex) { Console.WriteLine("=======>Dispose===>" + ex.Message); }
            RemoveGC();
        }


        private static void DumpLogicalTree(object parent, int level)
        {
            DependencyObject doParent = parent as DependencyObject;
            if (doParent == null) return;
            LocalValueEnumerator locallySetProperties = doParent.GetLocalValueEnumerator();
            while (locallySetProperties.MoveNext())
            {
                DependencyProperty propertyToClear = locallySetProperties.Current.Property;
                if (propertyToClear == null)
                    continue;
                if (!propertyToClear.ReadOnly) { doParent.ClearValue(propertyToClear); }
            }

            try
            {
                foreach (object child in LogicalTreeHelper.GetChildren(doParent))
                {
                    if (child == null)
                        continue;
                    DumpLogicalTree(child, level + 1);
                }
            }
            catch (Exception ex) { Console.WriteLine("=======>DumpLogicalTree===>" + ex.Message); }
        }


        private static void DumpVisualTree(DependencyObject parent, int level)
        {
            if (parent == null) return;
            LocalValueEnumerator locallySetProperties = parent.GetLocalValueEnumerator();
            while (locallySetProperties.MoveNext())
            {
                DependencyProperty propertyToClear = locallySetProperties.Current.Property;
                if (propertyToClear == null)
                    continue;
                if (!propertyToClear.ReadOnly) { parent.ClearValue(propertyToClear); }
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child == null)
                    continue;

                DumpVisualTree(child, level + 1);
            }
        }

        static List<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            List<T> list = new List<T>();
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        list.Add((T)child);
                    }

                    List<T> childItems = FindVisualChildren<T>(child);
                    if (childItems != null && childItems.Count() > 0)
                    {
                        foreach (var item in childItems)
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }


        static void ReleaseBinding(DependencyObject obj)
        {
            foreach (var v in FindVisualChildren<DependencyObject>(obj))
            {
                try
                {
                    var _idis = v as IDisposable;
                    if (_idis != null)
                    {
                        try
                        {
                            _idis.Dispose();
                        }
                        catch (Exception ex) { Console.WriteLine("=======>ReleaseBinding===>" + ex.Message); }
                    }
                    if (v is ContentControl)
                        ReleaseBinding(v);
                    //else
                    //    GetControl(v);

                    DumpLogicalTree(v, 0);
                    DumpVisualTree(v, 0);
                }
                catch (Exception ex) { Console.WriteLine("=======>main===>" + ex.Message); }
            }
            DumpLogicalTree(obj, 0);
            DumpVisualTree(obj, 0);
        }



    }
}