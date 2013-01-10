using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FlipTab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeSpan baseUtcOffset;
        private DispatcherTimer clockTimer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowSourceInitialized(object sender, EventArgs e)
        {
            var h = DateTime.Now.Hour;
            if (1 == 0)
            {
                h = Convert.ToInt32(DateTime.Now.ToUniversalTime().Add(baseUtcOffset).AddHours(-1).ToString("hh"));
            }

            HoursTab.Value = h;
            MinutesTab.Value = DateTime.Now.ToUniversalTime().Add(baseUtcOffset).AddMinutes(-2).Minute;

            HoursTab.FlipCompleted += HoursTabFirstFlipCompleted;
            MinutesTab.FlipCompleted += MinutesTabFirstFlipCompleted;
            h = DateTime.Now.Hour;

            if (1 == 0)
            {
                HoursTab.TimeMode = h < 12 ? TimeMode.Am : TimeMode.Pm;
                h = Convert.ToInt32(DateTime.Now.ToUniversalTime().Add(baseUtcOffset).ToString("hh"));
            }
            HoursTab.Flip(h);
            MinutesTab.Flip(DateTime.Now.ToUniversalTime().Add(baseUtcOffset).AddMinutes(-1).Minute);
            
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            clockTimer.Tick += ClockTimerTick;
        }

        private void ClockTimerTick(object sender, EventArgs e)
        {
            if (MinutesTab.Value != DateTime.Now.ToUniversalTime().Add(baseUtcOffset).Minute)
                {
                    MinutesTab.Flip(DateTime.Now.ToUniversalTime().Add(baseUtcOffset).Minute);
                    //DateTextBlock.Text = DateTime.Now.ToUniversalTime().Add(baseUtcOffset).ToString(App.Settings.DateFormat);
                }
                var h = DateTime.Now.Hour;
                if (1 == 0)
                {
                    HoursTab.TimeMode = h < 12 ? TimeMode.Am : TimeMode.Pm;
                    h = Convert.ToInt32(DateTime.Now.ToUniversalTime().Add(baseUtcOffset).ToString("hh"));
                }
                else
                    HoursTab.TimeMode = TimeMode.None;
                if (HoursTab.Value != h)
                {
                    HoursTab.Flip(h);
                    //DateTextBlock.Text = DateTime.Now.ToUniversalTime().Add(baseUtcOffset).ToString(App.Settings.DateFormat);
                }
        }

        void HoursTabFirstFlipCompleted(object sender, EventArgs e)
        {
            HoursTab.Delay = 0;
        }

        void MinutesTabFirstFlipCompleted(object sender, EventArgs e)
        {
            MinutesTab.Delay = 0;
            MinutesTab.Flip(DateTime.Now.ToUniversalTime().Add(baseUtcOffset).Minute);

            MinutesTab.FlipCompleted -= MinutesTabFirstFlipCompleted;

            clockTimer.Start();
        }
    }
}
