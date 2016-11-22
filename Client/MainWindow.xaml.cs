using System;
using System.IO;
using System.Windows;
using Server;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
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
        private MemoryStream _memoryStream = new MemoryStream();
        private readonly DispatcherTimer _timer = new System.Windows.Threading.DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(50)
        };
        private readonly DispatcherTimer _frameTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(1000)
        };

        /// <summary>
        /// Standard-Konstruktor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private Task t = null;
        private int framesPerSecond = 0;
        private void _timer_Tick(object sender, EventArgs e)
        {
            // Starts a new task to download the image if no task is running.
            if (t == null)
            {
                t = new Task(() => File.WriteAllBytes("temp.jpg", _imageManager.GetImage(NightVisionBox.IsChecked.Value)));
                t.Start();
            }

            // Skips if downloadtask isnt completed.
            if (!t.IsCompleted) return;

            // Sets downloaded image as Imagesource.
            Image.Source = _imageManager.ByteToImageSource(File.ReadAllBytes("temp.jpg"));
            Console.WriteLine(framesPerSecond++);
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
                ConnectedCheckBox.IsChecked = _imageManager.Connected && _controlManager.Connected;
                // Initiliase download timer.
                _timer.Tick += _timer_Tick;
                _timer.Start();
                // Initialise framerate timer.
                _frameTimer.Tick += (o, args) => framesPerSecond = 0;
                _frameTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controlManager.Connected)
            {
                _controlManager.Move(Global.MoveUp);
            }
            else ConnectedCheckBox.IsChecked = false;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controlManager.Connected)
            {
                _controlManager.Move(Global.MoveDown);
            }
            else ConnectedCheckBox.IsChecked = false;
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controlManager.Connected)
            {
                _controlManager.Move(Global.MoveLeft);
            }
            else ConnectedCheckBox.IsChecked = false;
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controlManager.Connected)
            {
                _controlManager.Move(Global.MoveRight);

            }
            else ConnectedCheckBox.IsChecked = false;
        }
    }
}