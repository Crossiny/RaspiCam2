using System;
using System.Windows;
using Server;
using System.Timers;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;

namespace Client
{
    // TODO: Use proper indication for connected

    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ControlManager _controlManager;
        private ImageManager _imageManager;
        private readonly DispatcherTimer _timer = new System.Windows.Threading.DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(1500)
        };

        /// <summary>
        /// Standard-Konstruktor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Image.Source = _imageManager.GetImage();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var imagePort = int.Parse(ImagePortBox.Text);
                var controlPort = int.Parse(ControlPortBox.Text);

                _imageManager = new ImageManager(HostBox.Text, imagePort);
                _controlManager = new ControlManager(HostBox.Text, controlPort);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ConnectedCheckBox.IsChecked = _imageManager.Connected && _controlManager.Connected;
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controlManager.Connected)
            {
                _controlManager.Move(Global.MoveUp);
                Image.Source = _imageManager.GetImage();
            }
            else ConnectedCheckBox.IsChecked = false;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controlManager.Connected)
            {
                _controlManager.Move(Global.MoveDown);
                Image.Source = _imageManager.GetImage();
            }
            else ConnectedCheckBox.IsChecked = false;
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controlManager.Connected)
            {
                _controlManager.Move(Global.MoveLeft);
                Image.Source = _imageManager.GetImage();
            }
            else ConnectedCheckBox.IsChecked = false;
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controlManager.Connected)
            {
                _controlManager.Move(Global.MoveRight);
                Image.Source = _imageManager.GetImage();

            }
            else ConnectedCheckBox.IsChecked = false;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (_imageManager.Connected)
            {
                Image.Source = _imageManager.GetImage();
            }
            else ConnectedCheckBox.IsChecked = false;
        }
    }
}