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
        private double xPosition = 0; 
        private double yPosition = 0;
        private double xSpeed = 0.1;   
        private double ySpeed = 0.1;
        private double Counter = 0;

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
            Timer.Tick += StartGameTick;
            Timer.Start();

        }
        private void StopGameMode()
        {
            if (isGameMode == true)
            {
                Counter = 0;
                BorderCounter.Text = Counter.ToString("0");
                Timer.Stop();
                Timer = null;
                isGameMode = false;
            }else
            {
                return;
            }

        }
        private void StartGameTick(object sender, EventArgs e)
        {
            double MaxWidth = CbxGridImages.ActualWidth - PakMeDan.ActualWidth;
            double MaxHeight = CbxGridImages.ActualHeight - PakMeDan.ActualHeight;

            xPosition += xSpeed;
            yPosition += ySpeed;
            if (xPosition < 0 || xPosition > MaxWidth)
            {
                xSpeed *= -1;
                Counter++;
            }

            if (yPosition < 0 || yPosition > MaxHeight)
            {
                ySpeed *= -1;
                Counter++;
            }

            PakMeDan.Margin = new Thickness(xPosition, yPosition, 0, 0);
            BorderCounter.Text = Counter.ToString();
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

        private void JeHebtMeGepakt(object sender, MouseEventArgs e)
        {
            if (isGameMode == true) {
            MessageBox.Show($"Je hebt er {Counter} over gedaan");
            Counter = 0;
            BorderCounter.Text = Counter.ToString("0");
            Timer.Stop();
            Timer = null;
            isGameMode = false;
            } else
            {
                return;
            }
        }
    }
}
