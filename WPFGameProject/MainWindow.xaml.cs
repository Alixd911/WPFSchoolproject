    using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private bool isGameMode = false;
        private DispatcherTimer Timer;
        private Random random = new Random();
        private double AnimationDuration = 1;
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
        private void StartStopGame()
        {
            if (!isGameMode) // check if party mode is active or not
            {
                isGameMode = true;
                StartGameMode();
                StartStopBTN.Content = "STOP!";
            }
            else
            {
                isGameMode = false;
                StopGameMode();
                StartStopBTN.Content = "START!";
            }
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PartyMode();
            }
            else if (e.Key == Key.End)
            {
                SpeedHandlerValue(); 
            }
            else if (e.Key == Key.Space)
            {
                StartStopGame();
            }
        }
        private void StartStopClick(object sender, RoutedEventArgs e)
        {
            StartStopGame();
        }
        private void partytime_Click(object sender, RoutedEventArgs e)
        {
            PartyMode();
        }
        private void SpeedHandlerClick(object sender, RoutedEventArgs e)
        {
            SpeedHandlerValue();
        }
        private void PartyMode()
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
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(AnimationDuration);
            Timer.Tick += PartyTimer_Tick;
            Timer.Start();
            // disabled de resize mode buttons dat je niet naar fullscreen kan gaan en kan geen minimalize
            ResizeMode = ResizeMode.NoResize;
            //zet de window state naar normal zodat je niet kan cheaten dat je met fullscreen geen party mode hebt
            WindowState= WindowState.Normal;

        }
        private void StopPartyMode()
        {
            if (Timer != null) 
            {
                Timer.Stop();
                Timer = null;
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

        private void StartGameMode()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(1);
            Timer.Tick += StartGameTick;
            Timer.Start();

        }
        private void StopGameMode()
        {
            if (Timer != null)
            {
                Timer.Stop();
                Timer = null;
            }
        }
        private void StartGameTick(object sender, EventArgs e)
        {
            double MaxWidth = CbxGridImages.MaxWidth;
            double MaxHeight = CbxGridImages.MaxHeight;

            double PositionWidthChange = (random.NextDouble() * 100);
            double PositionheightChange = (random.NextDouble() * 100);

            double newWidth = Math.Max(Width + PositionWidthChange, 100);
            double newHeight = Math.Max(Height + PositionheightChange, 100);

            newWidth = Math.Min(newWidth, MaxWidth);
            newHeight = Math.Min(newHeight, MaxHeight);
            var BounceImageAnimationTop = new DoubleAnimation();
            BounceImageAnimationTop.To = newHeight;
            BounceImageAnimationTop.Duration = TimeSpan.FromMilliseconds(AnimationDuration);

            var BounceImageAnimationLeft = new DoubleAnimation();
            BounceImageAnimationLeft.To = newWidth;
            BounceImageAnimationLeft.Duration = TimeSpan.FromMilliseconds(AnimationDuration);

            PakMeDan.BeginAnimation(Canvas.HeightProperty, BounceImageAnimationTop);
            PakMeDan.BeginAnimation(Canvas.WidthProperty, BounceImageAnimationLeft);
        }

        private void SpeedHandlerValue()
        {
            if (isPartyModeActive) {
            if(!IsFastSpeed)
            {
                AnimationDuration = 1;
                SpeedHandler.Content = "Faster";
            }
            else
            {
                AnimationDuration = 100;
                SpeedHandler.Content = "Slower";
            }
            IsFastSpeed = !IsFastSpeed;
            Timer.Interval = TimeSpan.FromMilliseconds(AnimationDuration);

            } else
            {
                MessageBox.Show("An error occured");
            }
        }
    }
}
