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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlipTab
{
    /// <summary>
    /// Interaction logic for FlipT.xaml
    /// </summary>
    public partial class FlipT : UserControl
    {
        const string Path = "/FlipTab;component/Resources/FlipClock/Digits/{0}.png";

        public event EventHandler FlipCompleted;

        private double speed = 1.0f;
        public double Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                var s = (Storyboard)this.Resources["FlipAnim"];
                ((DoubleAnimation)s.Children[0]).Duration = TimeSpan.FromSeconds(0.7f * value);
                ((DoubleAnimation)s.Children[1]).Duration = TimeSpan.FromSeconds(0.35f * value);
            }
        }

        private double delay = 0;
        public double Delay
        {
            get { return delay; }
            set
            {
                var s = (Storyboard)this.Resources["FlipAnim"];
                s.BeginTime = TimeSpan.FromSeconds(value);
                delay = value;
            }
        }

        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                BgLeftDigitTop.Source = new BitmapImage(new Uri(string.Format(Path, GetFirstDigit(value)), UriKind.Relative));
                BgRightDigitTop.Source = new BitmapImage(new Uri(string.Format(Path, GetLastDigit(value)), UriKind.Relative));
                BgLeftDigitBottom.Source = BgLeftDigitTop.Source;
                BgRightDigitBottom.Source = BgRightDigitTop.Source;
                _value = value;
            }
        }

        private TimeMode timeMode = TimeMode.None;
        public TimeMode TimeMode
        {
            get { return timeMode; }
            set
            {
                switch (value)
                {
                    case TimeMode.None:
                        AmPm.Visibility = System.Windows.Visibility.Hidden;
                        AmPmBack.Opacity = 0;
                        break;
                    case TimeMode.Am:
                        AmPm.Visibility = System.Windows.Visibility.Visible;
                        AmPmBack.Opacity = 1;
                        AmPmBack.ImageSource = new BitmapImage(new Uri("pack://application:,,,/FlipTab;component/Resources/FlipClock/am.png"));
                        break;
                    case TimeMode.Pm:
                        AmPm.Visibility = System.Windows.Visibility.Visible;
                        AmPmBack.Opacity = 1;
                        AmPmBack.ImageSource = new BitmapImage(new Uri("pack://application:,,,/FlipTab;component/Resources/FlipClock/pm.png"));
                        break;
                }
                timeMode = value;
            }
        }

        public FlipT()
        {
            InitializeComponent();

            //var bi = new BitmapImage();
            //bi.BeginInit();
            //bi.UriSource = new Uri(string.Format("pack://application:,,,/Clock;component/Resources/FlipClock/am.png"));
            //bi.EndInit();
            //AmPmBack.ImageSource = bi;
        }

        public void Flip(int d)
        {
            BgLeftDigitTop.Source = new BitmapImage(new Uri(string.Format(Path, GetFirstDigit(d)), UriKind.Relative));
            BgRightDigitTop.Source = new BitmapImage(new Uri(string.Format(Path, GetLastDigit(d)), UriKind.Relative));

            //if (timeMode != -1)
            //{
            //    if (timeMode == 0)
            //        AmPmBack.ImageSource = new BitmapImage(new Uri("am.png"));
            //    else
            //        AmPmBack.ImageSource = new BitmapImage(new Uri("pm.png"));
            //}

            if (TimeMode != TimeMode.None && GetFirstDigit(d) == 0)
            {
                BgLeftDigitGrid.Visibility = System.Windows.Visibility.Collapsed;
                LeftDigitBottomBrush.Opacity = 0;
                LeftDigitTopBrush.Opacity = 0;
                RightDigitTopTranslate.X = 0.285;
                RightDigitBottomTranslate.X = 0.285;
            }

            else
            {
                BgLeftDigitGrid.Visibility = System.Windows.Visibility.Visible;
                LeftDigitBottomBrush.Opacity = 1;
                LeftDigitTopBrush.Opacity = 1;

                RightDigitTopTranslate.X = 0.495;
                RightDigitBottomTranslate.X = 0.495;
            }
            //if (TimeMode == TimeMode.Am)
            //    AmPmBack.ImageSource = new BitmapImage(new Uri("am.png"));
            //else
            //    AmPmBack.ImageSource = new BitmapImage(new Uri("pm.png"));


            var s = (Storyboard)this.Resources["FlipAnim"];
            s.Begin();

            _value = d;
        }

        private static int GetFirstDigit(int n)
        {
            int result = n;
            if (result > 9)
            {
                return (int)(result / 10);
            }
            else
                return 0;
        }

        private static int GetLastDigit(int n)
        {
            int result = n;
            if (result > 9)
            {
                return GetRemainder(result, 10);
            }
            else
                return result;
        }

        private static int GetRemainder(int a, int b)
        {
            var result = (int)(a / b);
            return a - result * b;
        }

        private void FlipAnimCompleted(object sender, EventArgs e)
        {
            BgLeftDigitBottom.Source = BgLeftDigitTop.Source;
            BgRightDigitBottom.Source = BgRightDigitTop.Source;
            AmPm.Source = AmPmBack.ImageSource;

            if (FlipCompleted != null)
                FlipCompleted(this, EventArgs.Empty);
        }
    }
}
