using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace Client
{
    // TODO: Use proper indication for connected

    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Timer _butonLockTimer = new Timer
        {
            Interval = 500,
            AutoReset = false
        };

        private readonly DispatcherTimer _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(50)
        };

        private ControlManager _controlManager;
        private ImageManager _imageManager;
        private MemoryStream _memoryStream = new MemoryStream();

        private Task t;

        /// <summary>
        ///     Standard-Konstruktor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _butonLockTimer.Elapsed += RegisterButtonEvents;
        }

        private void RemoveButtonEvents()
        {
            UpButton.Click -= UpButton_Click;
            DownButton.Click -= DownButton_Click;
            LeftButton.Click -= LeftButton_Click;
            RightButton.Click -= RightButton_Click;
            ForwardButton.Click -= ForwardButton_Click;
            BackwardButton.Click -= BackwardButton_Click;
            TurnLeftButton.Click -= TurnLeftButton_Click;
            TurnRightButton.Click -= TurnRightButton_Click;
            _butonLockTimer.Start();
        }

        private void RegisterButtonEvents(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            UpButton.Click += UpButton_Click;
            DownButton.Click += DownButton_Click;
            LeftButton.Click += LeftButton_Click;
            RightButton.Click += RightButton_Click;
            ForwardButton.Click += ForwardButton_Click;
            BackwardButton.Click += BackwardButton_Click;
            TurnLeftButton.Click += TurnLeftButton_Click;
            TurnRightButton.Click += TurnRightButton_Click;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (t == null)
            {
                t = new Task(() => File.WriteAllBytes("temp.jpg", _imageManager.GetImage()));
                t.Start();
            }
            if (!t.IsCompleted) return;
            Image.Source = _imageManager.GetImage(File.ReadAllBytes("temp.jpg"));
            t = null;
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
            RemoveButtonEvents();
            if (_controlManager.Connected)
                _controlManager.MoveCamera(Server.Global.CameraUp);
            else ConnectedCheckBox.IsChecked = false;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonEvents();
            if (_controlManager.Connected)
                _controlManager.MoveCamera(Server.Global.CameraDown);
            else ConnectedCheckBox.IsChecked = false;
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonEvents();
            if (_controlManager.Connected)
                _controlManager.MoveCamera(Server.Global.CameraLeft);
            else ConnectedCheckBox.IsChecked = false;
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonEvents();
            if (_controlManager.Connected)
                _controlManager.MoveCamera(Server.Global.CameraRight);
            else ConnectedCheckBox.IsChecked = false;
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonEvents();
            if (_controlManager.Connected)
                _controlManager.MoveCamera(Server.Global.Forward);
            else ConnectedCheckBox.IsChecked = false;
        }

        private void BackwardButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonEvents();
            if (_controlManager.Connected)
                _controlManager.MoveCamera(Server.Global.Backward);
            else ConnectedCheckBox.IsChecked = false;
        }

        private void TurnLeftButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonEvents();
            if (_controlManager.Connected)
                _controlManager.MoveCamera(Server.Global.TurnLeft);
            else ConnectedCheckBox.IsChecked = false;
        }

        private void TurnRightButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonEvents();
            if (_controlManager.Connected)
                _controlManager.MoveCamera(Server.Global.TurnRight);
            else ConnectedCheckBox.IsChecked = false;
        }
    }
}