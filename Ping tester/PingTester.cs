using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Ping_tester
{
    class PingTester
    {
        public int TestTimeInMiliseconds { get; }
        public int TimeoutInMiliseconds { get; }

        private Ping pingTester = new Ping();

        public PingTester(int _TestTimeInMiliseconds, int _TimeoutInMiliseconds)
        {
            TestTimeInMiliseconds = _TestTimeInMiliseconds;
            TimeoutInMiliseconds = _TimeoutInMiliseconds;
        }

        // Główna metoda przeprowadzająca jeden test PING
        public async Task<PingReply> ConductTest(string address)
        {
            return await pingTester.SendPingAsync(address, TimeoutInMiliseconds);
        }

        // Metoda zwracająca stringa z wynikiem testu
        public string DisplayPingReply(PingReply pingReply)
        {
            if (pingReply != null)
            {
                return $"Ping status : {pingReply.Status}  IP : {pingReply.Address}  time = {pingReply.RoundtripTime} \n";
            }
            else
            {
                return "No reply. \n";
            }
        }

        // Metoda zwracająca stringa z informacją o zakończeniu testu
        public string DisplayTestFinish()
        {
            return "Test zakończony. \n";
        }
    }
}
