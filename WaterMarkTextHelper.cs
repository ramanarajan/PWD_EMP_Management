﻿/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * WaterMarkTextHelper file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 5/29/2017 2:00:17 PM
* ******************************************************************************/

using System.Windows;
using System.Windows.Controls;


namespace VollomeStudio.Helpers
{
    public class WaterMarkTextHelper : DependencyObject
    {
        #region Attached Properties

        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(WaterMarkTextHelper), new UIPropertyMetadata(false, OnIsMonitoringChanged));


        public static bool GetWatermarkText(DependencyObject obj)
        {
            return (bool)obj.GetValue(WatermarkTextProperty);
        }

        public static void SetWatermarkText(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkTextProperty, value);
        }

        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.RegisterAttached("WatermarkText", typeof(string), typeof(WaterMarkTextHelper), new UIPropertyMetadata(string.Empty));


        public static int GetTextLength(DependencyObject obj)
        {
            return (int)obj.GetValue(TextLengthProperty);
        }

        public static void SetTextLength(DependencyObject obj, int value)
        {
            obj.SetValue(TextLengthProperty, value);

            if (value >= 1)
                obj.SetValue(HasTextProperty, true);
            else
                obj.SetValue(HasTextProperty, false);
        }

        public static readonly DependencyProperty TextLengthProperty =
            DependencyProperty.RegisterAttached("TextLength", typeof(int), typeof(WaterMarkTextHelper), new UIPropertyMetadata(0));

        #endregion

        #region Internal DependencyProperty

        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            set { SetValue(HasTextProperty, value); }
        }

        private static readonly DependencyProperty HasTextProperty =
            DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(WaterMarkTextHelper), new FrameworkPropertyMetadata(false));

        #endregion

        #region Implementation

        static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox)
            {
                TextBox txtBox = d as TextBox;

                if ((bool)e.NewValue)
                {
                    txtBox.TextChanged += TextChanged;
                    TextChanged(txtBox, null);
                }
                else
                    txtBox.TextChanged -= TextChanged;
            }
            else if (d is PasswordBox)
            {
                PasswordBox passBox = d as PasswordBox;

                if ((bool)e.NewValue)
                {
                    passBox.PasswordChanged += PasswordChanged;
                    PasswordChanged(passBox, null);
                }
                else
                    passBox.PasswordChanged -= PasswordChanged;
            }
            else if (d is ComboBox)
            {
                ComboBox combobox = d as ComboBox;

                if ((bool)e.NewValue)
                {
                    combobox.SelectionChanged += combobox_SelectionChanged;
                    combobox_SelectionChanged(combobox, null);
                }
                else
                    combobox.SelectionChanged -= combobox_SelectionChanged;
            }
        }

        static void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox txtBox = sender as ComboBox;

            if (txtBox == null) return;
            if (txtBox.SelectedIndex > -1)
                SetTextLength(txtBox, (txtBox.SelectedValue ?? " ").ToString().Length);
            else
                SetTextLength(txtBox, (txtBox.Text ?? "").Length);
        }

        static void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            if (txtBox == null) return;
            SetTextLength(txtBox, txtBox.Text.Length);
        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passBox = sender as PasswordBox;
            if (passBox == null) return;
            SetTextLength(passBox, passBox.Password.Length);
        }

        #endregion
    }
}