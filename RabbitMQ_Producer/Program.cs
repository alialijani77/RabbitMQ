// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();




await channel.ExchangeDeclareAsync(exchange: "firstexchange", ExchangeType.Direct, false, false);
await channel.ExchangeDeclareAsync(exchange: "secondexchange", ExchangeType.Fanout, false, false);
await channel.QueueDeclareAsync(queue: "queue_two", durable: false, exclusive: false, autoDelete: false, arguments: null);
await channel.QueueDeclareAsync(queue: "queue_one", durable: false, exclusive: false, autoDelete: false, arguments: null);

await channel.ExchangeBindAsync("secondexchange", "firstexchange", "");

const string message = "Hello World!";
var body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: "firstexchange",routingKey : "", body: body);

Console.WriteLine("Message send");

Console.ReadLine();
