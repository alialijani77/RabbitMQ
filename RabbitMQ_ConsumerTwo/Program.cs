// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


Console.WriteLine("Cosumer_Two");
Console.WriteLine("====================================");



var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: "firstexchange", ExchangeType.Direct);


await channel.QueueDeclareAsync("queue_two");

await channel.QueueBindAsync(queue: "queue_two", exchange: "firstexchange", routingKey: "routingkey2");


var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
	var body = ea.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);
	Console.WriteLine($" [x] Received {message}");
};

await channel.BasicConsumeAsync("queue_two", true, consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
