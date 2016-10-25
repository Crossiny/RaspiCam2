using System;
using System.Windows;
using Server;

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

        public MainWindow()
        {
            InitializeComponent();
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