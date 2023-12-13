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
        private static TuyaApi tuyaApi;

        internal static void Start()
        {
            // Beolvassuk a felhasználó által megadott régiót
            int region = UserInput.GetRegion();
            // Beolvassuk a felhasználó által megadott azonosítót és titkos kódot
            var (accessId, apiSecret) = UserInput.GetLoginDetails();

            // Létrehozzuk a TuyaApi példányt a felhasználó által megadott adatokkal
            tuyaApi = new TuyaApi((TuyaApi.Region)region, accessId, apiSecret);

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
            TuyaDevice tuyaDevice = await DeviceInfo(e.GwId);
            TuyaDeviceStorage.AddDevice(tuyaDevice);
        }

        internal static async Task<TuyaDevice> DeviceInfo(string device)
        {
            TuyaDeviceApiInfo tuyaDeviceApiInfo = await tuyaApi.GetDeviceInfoAsync(device);
            Console.WriteLine($"Device name: {tuyaDeviceApiInfo.Name}");
            return new TuyaDevice(tuyaDeviceApiInfo.Ip, tuyaDeviceApiInfo.LocalKey, tuyaDeviceApiInfo.Id);
        }
    }
    internal static class UserInput //Bing AI generated
    {
        public static string GetDeviceName()
        {
            Console.WriteLine("Kérlek, add meg az eszköz nevét:");
            return Console.ReadLine();
        }

        public static string GetDeviceType()
        {
            Console.WriteLine("Kérlek, add meg az eszköz típusát:");
            return Console.ReadLine();
        }

        public static int GetRegion()
        {
            List<string> options = new List<string> { "China", "Western America", "Eastern America", "Central Europe", "Western Europe", "India" };

            string chooseOptionText = "Válassza ki az ön régióját alábbi lehetőségek közül:";
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

        public static int GetDevices()
        {
            string enterDevicesText = "Adja meg mennyi Tuya eszközzel rendelkezik numerikus formában: ";
            string input;
            int devices;
            while (true)
            {
                input = Console.ReadLine();
                bool isInteger = int.TryParse(input, out int intValue);
                if (isInteger)
                {
                    devices = intValue;
                    break;
                }
                else
                {
                    Console.WriteLine(enterDevicesText);
                }
            }
            return devices;
        }

        public static (string, string) GetLoginDetails()
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

    internal class TuyaDeviceStorage // Bing AI generated
    {
        private static Dictionary<string, TuyaDevice> devices = new Dictionary<string, TuyaDevice>();

        public static void AddDevice(TuyaDevice device)
        {
            string key = device.IP + device.DeviceId;
            if (!devices.ContainsKey(key))
            {
                devices.Add(key, device);
            }
        }

        public static TuyaDevice GetDevice(string ip, string deviceId)
        {
            string key = ip + deviceId;
            if (devices.ContainsKey(key))
            {
                return devices[key];
            }
            else
            {
                return null;
            }
        }
    }
}