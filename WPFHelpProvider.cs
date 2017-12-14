
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * WPFHelpProvider file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 08-Nov-17 7:20:48 PM
* ******************************************************************************/

using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace VollomeStudio.Helpers
{
    public static class WPFHelpProvider
    {

        #region Fields

        private static string mHelpNamespace = null;

        private static bool mShowHelp = false;

        private static string mNotFoundTopic;

        public static readonly DependencyProperty HelpKeywordProperty =

            DependencyProperty.RegisterAttached("HelpKeyword", typeof(string), typeof(WPFHelpProvider));

        public static readonly DependencyProperty HelpNavigatorProperty =

            DependencyProperty.RegisterAttached("HelpNavigator", typeof(HelpNavigator), typeof(WPFHelpProvider),

                                                new PropertyMetadata(HelpNavigator.TableOfContents));

        public static readonly DependencyProperty ShowHelpProperty =

            DependencyProperty.RegisterAttached("ShowHelp", typeof(bool), typeof(WPFHelpProvider));

        #endregion

        #region Constructors

        static WPFHelpProvider()
        {
            CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement),new CommandBinding(ApplicationCommands.Help, OnHelpExecuted, OnHelpCanExecute));
        }

        #endregion

        #region Public Properties

        public static string HelpNamespace

        {

            get { return mHelpNamespace; }

            set { mHelpNamespace = value; }

        }

        public static bool ShowHelp

        {

            get { return mShowHelp; }

            set { mShowHelp = value; }

        }

        public static string NotFoundTopic

        {

            get { return mNotFoundTopic; }

            set { mNotFoundTopic = value; }

        }

        #endregion

        #region Public Methods

        #region HelpKeyword

        public static string GetHelpKeyword(DependencyObject obj)

        {

            return (string)obj.GetValue(HelpKeywordProperty);

        }

        public static void SetHelpKeyword(DependencyObject obj, string value)

        {

            obj.SetValue(HelpKeywordProperty, value);

        }

        #endregion

        #region HelpNavigator

        public static HelpNavigator GetHelpNavigator(DependencyObject obj)

        {

            return (HelpNavigator)obj.GetValue(HelpNavigatorProperty);

        }

        public static void SetHelpNavigator(DependencyObject obj, HelpNavigator value)

        {

            obj.SetValue(HelpNavigatorProperty, value);

        }

        #endregion

        #region ShowHelp

        public static bool GetShowHelp(DependencyObject obj)

        {

            return (bool)obj.GetValue(ShowHelpProperty);

        }

        public static void SetShowHelp(DependencyObject obj, bool value)

        {

            obj.SetValue(ShowHelpProperty, value);

        }

        #endregion

        #endregion

        #region Private Members

        private static void OnHelpCanExecute(object sender, CanExecuteRoutedEventArgs e)

        {

            e.CanExecute = CanExecuteHelp((DependencyObject)sender) || ShowHelp;

        }

        private static bool CanExecuteHelp(DependencyObject sender)

        {

            if (sender != null)

            {

                if (GetShowHelp(sender))

                    return true;

                return CanExecuteHelp(VisualTreeHelper.GetParent(sender));

            }

            return false;

        }

        private static DependencyObject GetHelp(DependencyObject sender)

        {

            if (sender != null)

            {

                if (GetShowHelp(sender))

                    return sender;

                return GetHelp(VisualTreeHelper.GetParent(sender));

            }

            return null;

        }

        private static void OnHelpExecuted(object sender, ExecutedRoutedEventArgs e)

        {

            DependencyObject ctl = GetHelp(sender as DependencyObject);

            if (ctl != null && GetShowHelp(ctl))

            {

                string parameter = GetHelpKeyword(ctl);

                HelpNavigator command = GetHelpNavigator(ctl);

                if (!e.Handled && !string.IsNullOrEmpty(HelpNamespace))

                {

                    if (!string.IsNullOrEmpty(parameter))

                    {

                        Help.ShowHelp(null, HelpNamespace, command, parameter);

                        e.Handled = true;

                    }

                    if (!e.Handled)

                    {

                        Help.ShowHelp(null, HelpNamespace, command);

                        e.Handled = true;

                    }

                }

                if (!e.Handled && !string.IsNullOrEmpty(HelpNamespace))

                {

                    Help.ShowHelp(null, HelpNamespace);

                    e.Handled = true;

                }

            }

            else if (ShowHelp)

            {

                if (!string.IsNullOrEmpty(NotFoundTopic))

                    Help.ShowHelp(null, HelpNamespace, NotFoundTopic);

                else

                    Help.ShowHelp(null, HelpNamespace);

                e.Handled = true;

            }

        }

        #endregion

    }
}