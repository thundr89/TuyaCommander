using com.clusterrr.TuyaNet;

namespace TuyaCommander
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DetectTuya.Start();
        }
    }
    internal class DetectTuya
    {
        private static readonly TuyaScanner tuyascanner = new TuyaScanner();
        private static TuyaApi tuyaapi;

        internal static void Start()
        {
            // Beolvassuk a felhasználó által megadott régiót
            int region = TuyaRegionSetup.Run();
            // Beolvassuk a felhasználó által megadott azonosítót és titkos kódot
            var (accessId, apiSecret) = TuyaLogin.Run();

            // Létrehozzuk a TuyaApi példányt a felhasználó által megadott adatokkal
            tuyaapi = new TuyaApi((TuyaApi.Region)region, accessId, apiSecret);

            tuyascanner.OnNewDeviceInfoReceived += TuyaDeviceFound;
            tuyascanner.Start();
        }

        internal static void Stop()
        {
            tuyascanner.OnNewDeviceInfoReceived -= TuyaDeviceFound;
            tuyascanner.Stop();
        }

        internal static async void TuyaDeviceFound(object sender, TuyaDeviceScanInfo e)
        {
            Console.WriteLine($"New device found! IP: {e.IP}, ID: {e.GwId}, version: {e.Version}");
        }
    }
    internal static class TuyaRegionSetup //Bing AI generated
    {
        public static int Run()
        {
            List<string> options = new List<string> { "China", "Western America", "Eastern America", "Central Europe", "Western Europe", "India" };

            string chooseOptionText = "Válasszon az alábbi lehetőségek közül:";
            string enterChoiceText = "Adja meg a választás számát: ";
            string invalidChoiceText = "Érvénytelen választás. Próbálja újra.";
            string chosenOptionText = "A választott lehetőség: ";

            Console.WriteLine(chooseOptionText);
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i}: {options[i]}");
            }

            int selection;
            while (true)
            {
                Console.Write(enterChoiceText);
                string input = Console.ReadLine();
                if (int.TryParse(input, out selection) && selection >= 0 && selection < options.Count)
                {
                    Console.WriteLine($"{chosenOptionText} {options[selection]}");
                    break;
                }
                else
                {
                    Console.WriteLine(invalidChoiceText);
                }
            }

            return selection;
        }
    }
    internal static class TuyaLogin //Bing AI generated
    {
        public static (string, string) Run()
        {
            string enterIdText = "Adja meg az azonosítóját: ";
            string enterSecretCodeText = "Adja meg a titkos kódját: ";
            string invalidInputText = "Üres válasz nem elfogadott. Próbálja újra.";

            string id;
            while (true)
            {
                Console.Write(enterIdText);
                id = Console.ReadLine();
                if (!string.IsNullOrEmpty(id))
                {
                    break;
                }
                else
                {
                    Console.WriteLine(invalidInputText);
                }
            }

            string secretCode;
            while (true)
            {
                Console.Write(enterSecretCodeText);
                secretCode = Console.ReadLine();
                if (!string.IsNullOrEmpty(secretCode))
                {
                    break;
                }
                else
                {
                    Console.WriteLine(invalidInputText);
                }
            }

            return (id, secretCode);
        }
    }
}