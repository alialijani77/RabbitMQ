// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();




await channel.ExchangeDeclareAsync(exchange: "firstexchange", ExchangeType.Direct);
await channel.ExchangeDeclareAsync(exchange: "secondexchange", ExchangeType.Fanout);

await channel.ExchangeBindAsync("secondexchange", "firstexchange", "routingkey1");

const string message = "Hello World!";
var body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: "firstexchange", routingKey: "routingkey1", body: body);

Console.WriteLine("Message send");

Console.ReadLine();
