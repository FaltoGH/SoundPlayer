using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SoundPlayer
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlaying;
        private readonly MediaPlayer player = new MediaPlayer();
        private readonly DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer(TimeSpan.FromMilliseconds(998),
                DispatcherPriority.SystemIdle,
                new EventHandler(Tick),
                Dispatcher)
            { IsEnabled = false };

            player.MediaOpened += Player_MediaOpened;
            player.MediaEnded += Player_MediaEnded;
            sldVolume.ValueChanged += SldVolume_ValueChanged;
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            isPlaying = false;
            btnPlay.Content = '▶';
            sldPosition.Value = sldPosition.Maximum;
            timer.Stop();
        }

        private void SldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = e.NewValue;
        }

        private void Player_MediaOpened(object sender, EventArgs e)
        {
            sldPosition.Maximum = player.NaturalDuration.TimeSpan.TotalMilliseconds;
        }


        private void Tick(object sender, EventArgs e)
        {
            SyncPosition();
        }
        
        private void SyncPosition()
        {
            if (!sldPosition.IsMouseCaptureWithin)
            {
                sldPosition.Value = player.Position.TotalMilliseconds;
            }
        }


        private void play_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if (player.HasAudio)
            {
                if (isPlaying)
                {
                    player.Pause();
                    b.Content = '▶';
                    timer.Stop();
                }
                else
                {
                    if (player.NaturalDuration.TimeSpan == player.Position)
                    {
                        player.Position = TimeSpan.Zero;
                        sldPosition.Value = 0;
                    }
                    player.Play();
                    timer.Start();
                    b.Content = "||";
                }
                isPlaying = !isPlaying;
            }
        }


        private void stop_Click(object sender, RoutedEventArgs e)
        {
            isPlaying = false;
            btnPlay.Content = '▶';
            sldPosition.Value = 0;
            timer.Stop();
            player.Stop();
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                player.Open(new Uri(dialog.FileName));
                isPlaying = false;
                btnPlay.Content = '▶';
                sldPosition.Value = 0;
                timer.Stop();
            }
        }

        private void sldPosition_LostMouseCapture(object sender, MouseEventArgs e)
        {
            Slider s = sender as Slider;
            player.Position = TimeSpan.FromMilliseconds(s.Value);
        }

        private void speaker_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (player.IsMuted)
            {
                b.Content = "🔊";
            }
            else
            {
                b.Content = "🔇";
            }
            player.IsMuted = !player.IsMuted;
        }
    }

    public class MsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double ms = (double)value;
            int m = (int)(ms / 1000 / 60);
            int s = (int)(ms / 1000 % 60);
            return $"{m}:{s:d2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class M100Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)((double)value * 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
