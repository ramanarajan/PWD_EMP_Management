
/********************************************************************************
Copyright (C) 2017 Nettr Group
* * * * uc_Timer file is part of NSF Core. 
* * * * NSF Core can not be copied and/or distributed without the express 
* * * * permission of Nettr Group
* * * * Date 12-Sep-17 12:42:55 PM
* ******************************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VollomeStudio.Helpers
{
    /// <summary>
    /// Interaction logic for uc_Timer.xaml
    /// </summary>
    public partial class uc_Timer : UserControl
    {
        public uc_Timer()
        {
            InitializeComponent();
            AddIncreaseCommand();
            AddDecreaseCommand();
        }



        public long MaxMilliseconds
        {
            get { return (long)GetValue(MaxMillisecondsProperty); }
            set { SetValue(MaxMillisecondsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxMilliseconds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxMillisecondsProperty =
            DependencyProperty.Register("MaxMilliseconds", typeof(long), typeof(uc_Timer), new PropertyMetadata(60 * 1000L));


        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(TimeSpan), typeof(uc_Timer),
        new UIPropertyMetadata(TimeSpan.FromMilliseconds(0), new PropertyChangedCallback(OnValueChanged)));



        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            uc_Timer control = obj as uc_Timer;
            TimeSpan ts = new TimeSpan();
            if (e.NewValue is TimeSpan)
                ts = (TimeSpan)e.NewValue;
            else
                ts = TimeSpan.FromMilliseconds((double)e.NewValue);
            control.Hours = ts.Hours;
            control.Minutes = ts.Minutes;
            control.Seconds = ts.Seconds;
        }



        public double ValueInMilliseconds
        {
            get { return (double)GetValue(ValueInMillisecondsProperty); }
            set { SetValue(ValueInMillisecondsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueInMilliseconds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueInMillisecondsProperty =
            DependencyProperty.Register("ValueInMilliseconds", typeof(double), typeof(uc_Timer), new UIPropertyMetadata(0.0, new PropertyChangedCallback(OnValueChanged)));



        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }
        public static readonly DependencyProperty HoursProperty =
        DependencyProperty.Register("Hours", typeof(int), typeof(uc_Timer),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        public static readonly DependencyProperty MinutesProperty =
        DependencyProperty.Register("Minutes", typeof(int), typeof(uc_Timer),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        public static readonly DependencyProperty SecondsProperty =
        DependencyProperty.Register("Seconds", typeof(int), typeof(uc_Timer),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));


        private static void OnTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            uc_Timer control = obj as uc_Timer;
            if (control.Seconds == 60)
            {
                control.Seconds = 0;
                control.Minutes += 1;
                
            }
            if (control.Seconds == -1)
            {
                control.Seconds = 59;
                control.Minutes -= 1;
            }
            
            if (control.Hours == 99)
                control.Hours = 0;

            TimeSpan t = new TimeSpan(control.Hours, control.Minutes, control.Seconds);
            if (t < TimeSpan.FromMilliseconds(control.MaxMilliseconds))
                if (Math.Sign(t.TotalMilliseconds) == -1)
                    control.Value = new TimeSpan();
                else
                    control.Value = t;
            else
                control.Value = TimeSpan.FromMilliseconds(control.MaxMilliseconds);
            control.ValueInMilliseconds = control.Value.TotalMilliseconds;
        }

       

        private void Update(string _name,bool _increase)
        {
            switch (_name)
            {
                case "sec":
                    if (_increase)
                        this.Seconds++;
                    else
                        this.Seconds--;
                    break;

                case "min":
                    if (_increase)
                        this.Minutes++;
                    else
                        this.Minutes--;
                    break;

                case "hour":
                    if (_increase)
                        this.Hours++;
                    else
                        this.Hours--;
                    break;
            }
        }

        public ICommand IncreaseCommand { private set; get; }


        /// <summary>
        /// add Increase timer
        /// </summary>
        public void AddIncreaseCommand()
        {
            IncreaseCommand = new RelayCommandWithParameter(IncreaseCommandExcecute, CanExcecuteIncreaseCommand);
        }

        /// <summary>
        /// can Exceute Increase timer command 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExcecuteIncreaseCommand(object obj)
        {
            // check is running
            return Value<TimeSpan.FromMilliseconds(MaxMilliseconds);
        }


        /// <summary>
        /// Exceute Increase timer command
        /// </summary>
        /// <param name="obj"></param>
        private void IncreaseCommandExcecute(object obj)
        {
            Update(obj as string, true);
        }

        public ICommand DecreaseCommand { private set; get; }


        /// <summary>
        /// add Decrease timer
        /// </summary>
        public void AddDecreaseCommand()
        {
            DecreaseCommand = new RelayCommandWithParameter(DecreaseCommandExcecute, CanExcecuteDecreaseCommand);
        }

        /// <summary>
        /// can Exceute Decrease timer command 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExcecuteDecreaseCommand(object obj)
        {
            // check is running
            return Value <= TimeSpan.FromMilliseconds(MaxMilliseconds) && Value.TotalMilliseconds>0;
        }


        /// <summary>
        /// Exceute Increase timer command
        /// </summary>
        /// <param name="obj"></param>
        private void DecreaseCommandExcecute(object obj)
        {
            Update(obj as string, false);
        }


        private void _MouseWheel(object sender, MouseWheelEventArgs e)
        {
            string str = ((TextBox)sender).Tag as string;
            if (e.Delta > 0)
            {
                if (CanExcecuteIncreaseCommand(null))
                    Update(str, true);

            }
            else
            {
                if (CanExcecuteDecreaseCommand(null))
                    Update(str, false);
            }
        }
    }
}
