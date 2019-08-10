using System.Net.NetworkInformation;
using System.Timers;
using System.Windows;

namespace Ping_tester
{
    public partial class MainWindow : Window
    {
        // Stworzenie obiektu PingTestera z czasem testu PING trwającym 7 sekund
        // oraz limitem czasu PING wynoszącym 5 sekund
        // Możliwość zmiany czasu testu z poziomu kodu
        private PingTester pingTester = new PingTester(7000, 5000);
        private Timer timer = new Timer(100.0);

        // Konfiguracja czasmierza i zdarzeń
        public MainWindow()
        {
            InitializeComponent();
            setButtonContent();
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            progressBar.ValueChanged += ProgressBar_ValueChanged;  
        }

        // Aktywacja czasomierza
        private void setTimer()
        {
            timer.Enabled = true;
        }

        // Dezaktywacja czasomierza
        private void stopTimer()
        {
            timer.Stop();
        }

        // Dezaktywacja UI na czas testu
        private void setUiToBusyMode()
        {
            testOutput.Text = "Odpowiedzi PING: \n";
            progressBar.Value = progressBar.Minimum;
            adressInput.IsEnabled = false;
            startTestBtn.IsEnabled = false;
        }

        // Aktywacja UI
        private void setUiToStanbyMode()
        {
            adressInput.IsEnabled = true;
            startTestBtn.IsEnabled = true;
        }

        // Dostoswanie właściwości przycisku w przypadku zmiany czasu testu PING z 7 sekund na inny
        private void setButtonContent()
        {
            progressBar.Maximum = pingTester.TestTimeInMiliseconds / 100;
            double numOfSeconds = pingTester.TestTimeInMiliseconds / 1000;
            startTestBtn.Content = $"Wysyłaj zapytania PING przez {numOfSeconds} sek.";
        }

        // Wydarzenie występujące co 0,1 sekundy, aktualizujące pasek ładowania
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => progressBar.Value++);
        }

        // Wydarzenie sprawdzające poziom załadowania paska
        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (progressBar.Value == progressBar.Maximum)
            {
                stopTimer();
                setUiToStanbyMode();
            }
        }

        // Uruchomienie testu PING
        private async void StartTestBtn_Click(object sender, RoutedEventArgs e)
        {
            PingReply pingReply;
            long timeFromStart = 0;
            setUiToBusyMode();
            setTimer();
            while (timeFromStart <= pingTester.TestTimeInMiliseconds)
            {
                pingReply = await pingTester.ConductTest(adressInput.Text);
                timeFromStart += pingReply.RoundtripTime;
                testOutput.Text += pingTester.DisplayPingReply(pingReply);
            }
            testOutput.Text += pingTester.DisplayTestFinish();
        }
    }
}
