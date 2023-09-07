using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WPFGameProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPartyModeActive = false;
        private DispatcherTimer partyTimer;
        private Random random = new Random();
        public MainWindow()
        { 
            InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void CbxGridImagesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Currentitem.SelectedIndex > -1 && PakMeDan !=null)
            {
                ComboBoxItem comboxItemImage = (ComboBoxItem)Currentitem.SelectedItem;
                string ComboBoxItem = comboxItemImage.Name;
                switch (ComboBoxItem)
                {
                    case "TheRock":
                        PakMeDan.Source = new BitmapImage(new Uri("/Assets/Therock.jpg", UriKind.Relative));
                        break;
                    case "RicoVerhoeven":
                        PakMeDan.Source = new BitmapImage(new Uri("Assets/Ricoverhoeven.jpg", UriKind.Relative));
                        break;
                    case "JohnCena":
                        PakMeDan.Source = new BitmapImage(new Uri("Assets/johncena.jpg", UriKind.Relative));
                        break;
                    default:
                        MessageBox.Show("An error occured");
                        break;
                }
            }

        }
        private void StartStopGame(object sender, RoutedEventArgs e)
        {
            if (StartStopBTN.Content.ToString() == "START!")
            {
                StartStopBTN.Content = "STOP!";
            }
            else
            {
                StartStopBTN.Content = "START!";
            }
        }

        private void PartyMode(object sender, RoutedEventArgs e)
        {
            if (!isPartyModeActive)
            {
                isPartyModeActive = true;
                StartPartyMode();
                partytime.Content = "PARTY MODE ON";
            }
            else
            {
                isPartyModeActive = false;
                StopPartyMode();
                partytime.Content = "PARTY MODE OFF";
            }
        }

        private void StartPartyMode()
        {
            partyTimer = new DispatcherTimer();
            partyTimer.Interval = TimeSpan.FromMilliseconds(0);
            partyTimer.Tick += PartyTimer_Tick;
            partyTimer.Start();
            ResizeMode = ResizeMode.NoResize;
        }
        private void StopPartyMode()
        {
            if (partyTimer != null)
            {
                partyTimer.Stop();
                partyTimer = null;
                ResizeMode = ResizeMode.CanResize;
            }
        }
        private void PartyTimer_Tick(object sender, EventArgs e)
        {
            double MaxWidth = SystemParameters.PrimaryScreenWidth;
            double MaxHeight = SystemParameters.PrimaryScreenHeight;

            double widthChange = (random.NextDouble() * 400) - 200; 
            double heightChange = (random.NextDouble() * 400) - 200; 

            double newWidth = Math.Max(Width + widthChange, 100);
            double newHeight = Math.Max(Height + heightChange, 100);

            newWidth = Math.Min(newWidth, MaxWidth);
            newHeight = Math.Min(newHeight, MaxHeight);

            var bounceWidthAnimation = new DoubleAnimation();
            bounceWidthAnimation.From = this.Width;
            bounceWidthAnimation.To = newWidth;
            bounceWidthAnimation.Duration = TimeSpan.FromSeconds(0.1); 

            var bounceHeightAnimation = new DoubleAnimation();
            bounceHeightAnimation.From = this.Height;
            bounceHeightAnimation.To = newHeight;
            bounceHeightAnimation.Duration = TimeSpan.FromSeconds(0.1); 

            var storyboard = new Storyboard();
            storyboard.Children.Add(bounceWidthAnimation);
            storyboard.Children.Add(bounceHeightAnimation);
            Storyboard.SetTarget(bounceWidthAnimation, this);
            Storyboard.SetTargetProperty(bounceWidthAnimation, new PropertyPath(Window.WidthProperty));
            Storyboard.SetTarget(bounceHeightAnimation, this);
            Storyboard.SetTargetProperty(bounceHeightAnimation, new PropertyPath(Window.HeightProperty));
            storyboard.Begin();
        }
    }
}
