using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Threading.Tasks;

namespace IoTMessageProcessor
{
    class Program
    {
        static async Task Main(string[] args)
        {           
            var consumerGroup = PartitionReceiver.DefaultConsumerGroupName;

            var processor = new EventProcessorHost(
                Config.hubName,
                consumerGroup,
                Config.iotHubConnectionString,
                Config.storageConnectionString,
                Config.storageContainerName);

            await processor.RegisterEventProcessorAsync<LoggingEventProcessor>();

            Console.WriteLine("Event processor started, press any key to exit...");
            Console.ReadLine();

            await processor.UnregisterEventProcessorAsync();
        }
    }
}
