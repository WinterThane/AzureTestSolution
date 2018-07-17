using System;
using System.Text;
using System.Threading.Tasks;
using AzureTest.Common;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace TestAgent
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Initializing test agent...");

            var device = DeviceClient.CreateFromConnectionString(Config.DeviceConnectionString);

            await device.OpenAsync();

            Console.WriteLine("Device is connected!");
            await UpdateTwin(device);

            Console.WriteLine("Press a key to perform an action:");
            Console.WriteLine("q: quit");
            Console.WriteLine("h: send happy feedback");
            Console.WriteLine("u: send unhappy feedback");
            Console.WriteLine("e: send emergency help");

            var random = new Random();
            var quitRequested = false;
            while(!quitRequested)
            {
                Console.Write("Action? ");
                var input = Console.ReadKey().KeyChar;
                Console.WriteLine();

                var status = StatusType.NotSpecified;
                var latitude = random.Next(0, 100);
                var longitude = random.Next(0, 100);

                switch(Char.ToLower(input))
                {
                    case 'q':
                        quitRequested = true;
                        break;
                    case 'h':
                        status = StatusType.Happy;
                        break;
                    case 'u':
                        status = StatusType.Unhappy;
                        break;
                    case 'e':
                        status = StatusType.Emergency;
                        break;
                }

                var telemetry = new Telemetry
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    Status = status
                };

                var payload = JsonConvert.SerializeObject(telemetry);
                var message = new Message(Encoding.ASCII.GetBytes(payload));

                await device.SendEventAsync(message);

                Console.WriteLine("Message sent.");
            }
        }

        private static async Task UpdateTwin(DeviceClient device)
        {
            var twinProperties = new TwinCollection();
            twinProperties["connectionType"] = "wi-fi";
            twinProperties["connectionStrength"] = "full";

            await device.UpdateReportedPropertiesAsync(twinProperties);
        }
    }
}
