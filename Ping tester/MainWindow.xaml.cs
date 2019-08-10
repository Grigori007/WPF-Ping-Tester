using System.Net.NetworkInformation;
using System.Timers;
using System.Windows;

namespace Ping_tester
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PingTester pingTester = new PingTester(7000, 5000);
        private Timer timer = new Timer(100.0);

        public MainWindow()
        {
            InitializeComponent();

            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            progressBar.ValueChanged += ProgressBar_ValueChanged;  
        }

        private void setTimer()
        {
            timer.Enabled = true;
        }

        private void stopTimer()
        {
            timer.Stop();
        }

        private void setUiToBusyMode()
        {
            testOutput.Text = "Odpowiedzi PING: \n";
            progressBar.Value = progressBar.Minimum;
            adressInput.IsEnabled = false;
            startTestBtn.IsEnabled = false;
        }

        private void setUiToStanbyMode()
        {
            adressInput.IsEnabled = true;
            startTestBtn.IsEnabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => progressBar.Value++);
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (progressBar.Value == progressBar.Maximum)
            {
                stopTimer();
                setUiToStanbyMode();
            }
        }

        private async void StartTestBtn_Click(object sender, RoutedEventArgs e)
        {
            long timeFromStart = 0;
            setUiToBusyMode();
            setTimer();
            while (timeFromStart <= pingTester.TestTimeInMiliseconds)
            {
                PingReply pingReply = await pingTester.ConductTest(adressInput.Text);
                timeFromStart += pingReply.RoundtripTime;
                testOutput.Text += pingTester.DisplayPingReply(pingReply);
            }
        }
    }
}
