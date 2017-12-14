
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * ProgressRing file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 26-Sep-17 12:38:08 PM
* ******************************************************************************/

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VollomeStudio.Helpers
{
    /// <summary>
    /// Interaction logic for ProgressRing.xaml
    /// </summary>
    public partial class ProgressRing : UserControl
    {
        public ProgressRing()
        {
            InitializeComponent();

        }

        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register("TextForeground", typeof(Brush), typeof(ProgressRing), new PropertyMetadata(Brushes.Black));




        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Percentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(double), typeof(ProgressRing), new PropertyMetadata(0.0));




        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }



        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true));



        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(ProgressRing), new PropertyMetadata("Loading"));



        public bool EnableStaus
        {
            get { return (bool)GetValue(EnableStausProperty); }
            set { SetValue(EnableStausProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableStaus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableStausProperty =
            DependencyProperty.Register("EnableStaus", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true));



        public bool EnablePercentage
        {
            get { return (bool)GetValue(EnablePercentageProperty); }
            set { SetValue(EnablePercentageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnablePercentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnablePercentageProperty =
            DependencyProperty.Register("EnablePercentage", typeof(bool), typeof(ProgressRing), new PropertyMetadata(false));


    }
}
