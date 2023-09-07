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
        private double AnimationDuration = 5;
        private bool IsFastSpeed = false;
        public MainWindow()
        { 
            InitializeComponent();
            //center application in middle of screen when starting up
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void CbxGridImagesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Currentitem.SelectedIndex > -1 && PakMeDan !=null)
            {
                ComboBoxItem comboxItemImage = (ComboBoxItem)Currentitem.SelectedItem;
                string ComboBoxItem = comboxItemImage.Name;
                switch (ComboBoxItem) // check on which character is chosen
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
            if (!isPartyModeActive) // check if party mode is active or not
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
            partyTimer.Interval = TimeSpan.FromMilliseconds(AnimationDuration);
            partyTimer.Tick += PartyTimer_Tick;
            partyTimer.Start();
            // disabled de resize mode buttons dat je niet naar fullscreen kan gaan en kan geen minimalize
            ResizeMode = ResizeMode.NoResize;
            //zet de window state naar normal zodat je niet kan cheaten dat je met fullscreen geen party mode hebt
            WindowState= WindowState.Normal;

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
            bounceWidthAnimation.To = newWidth;
            bounceWidthAnimation.Duration = TimeSpan.FromMilliseconds(AnimationDuration); 

            var bounceHeightAnimation = new DoubleAnimation();
            bounceHeightAnimation.To = newHeight;
            bounceHeightAnimation.Duration = TimeSpan.FromMilliseconds(AnimationDuration); 

            BeginAnimation(Window.WidthProperty, bounceWidthAnimation);
            BeginAnimation(Window.HeightProperty, bounceHeightAnimation);
        }

        private void SpeedHandlerValue(object sender, EventArgs e)
        {
            if (isPartyModeActive) {
            if(IsFastSpeed)
            {
                AnimationDuration = 1;
                SpeedHandler.Content = "Slower";
            }
            else
            {
                AnimationDuration = 100;
                SpeedHandler.Content = "Faster";
            }
            IsFastSpeed = !IsFastSpeed;
            partyTimer.Interval = TimeSpan.FromMilliseconds(AnimationDuration);

            } else
            {
                MessageBox.Show("An error occured");
            }
        }
    }
}
