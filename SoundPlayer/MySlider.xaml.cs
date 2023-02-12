using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace SoundPlayer
{
    /// <summary>
    /// Interaction logic for MySlider.xaml
    /// </summary>
    public partial class MySlider : Slider
    {
        public MySlider()
        {
            InitializeComponent();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var thumb = (Template.FindName("PART_Track", this) as Track).Thumb;
                thumb.RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left)
                {
                    RoutedEvent = MouseLeftButtonDownEvent,
                    Source = e.Source
                });
            }
        }
    }
}
