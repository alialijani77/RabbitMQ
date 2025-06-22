// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


Console.WriteLine("Cosumer_One");
Console.WriteLine("====================================");



var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: "secondexchange", ExchangeType.Fanout);


await channel.QueueDeclareAsync("queue_one");

await channel.QueueBindAsync(queue: "queue_one", exchange: "secondexchange", routingKey: "");


var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
	var body = ea.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);
	Console.WriteLine($" [x] Received {message}");
};

await channel.BasicConsumeAsync("queue_one", true, consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();