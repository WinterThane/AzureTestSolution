using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

            var twinProperties = new TwinCollection();
            twinProperties["connectionType"] = "wi-fi";
            twinProperties["connectionStrength"] = "weak";

            await device.UpdateReportedPropertiesAsync(twinProperties);


            //var counter = 0;

            //while(counter < 3)
            //{
            //    var telemetry = new Telemetry
            //    {
            //        Message = "Sending complex objext...",
            //        StatusCode = counter
            //    };

            //    var telemetryJson = JsonConvert.SerializeObject(telemetry);

            //    var message = new Message(Encoding.ASCII.GetBytes(telemetryJson));

            //    await device.SendEventAsync(message);

            //    Console.WriteLine("Message sent (to the cloud).");

            //    counter++;
            //    Thread.Sleep(2000);
                
            //}

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
