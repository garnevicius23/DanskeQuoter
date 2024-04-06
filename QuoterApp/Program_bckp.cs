using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using QuoterApp.Model;
using QuoterApp.DataAccess;

namespace QuoterApp
{
    class Program_bckp
    {
        static void Main(string[] args)
        {
            var consumer = new OrderPublisher();
            consumer.Event += HandleEvent;
            consumer.StartEventListener(new HardcodedMarketOrderSource());

            /*var gq = new YourQuoter();
            var qty = 120;

            var quote = gq.GetQuote("DK50782120", qty);
            var vwap = gq.GetVolumeWeightedAveragePrice("DK50782120");

            Console.WriteLine($"Quote: {quote}, {quote / (double)qty}");
            Console.WriteLine($"Average Price: {vwap}");
            Console.WriteLine();
            Console.WriteLine($"Done");*/

            //var factory = new ConnectionFactory { HostName = "localhost", Port = 5672, UserName = "admin", Password = "admin" };
            //using var connection = factory.CreateConnection();
            //using var channel = connection.CreateModel();

/*            channel.QueueDeclare(queue: "test",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<MarketOrder>(body);
                Console.WriteLine($" [x] Received {message.InstrumentId}; {message.Quantity}: {message.Price}");
            };
            channel.BasicConsume(queue: "test",
                                 autoAck: true,
                                 consumer: consumer);*/

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public static void HandleEvent(object sender, MarketOrderEvent e)
        {
            Console.WriteLine(e.InstrumentId);
        }
    }
}
