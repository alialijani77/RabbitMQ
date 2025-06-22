// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


Console.WriteLine("Cosumer_One");
Console.WriteLine("====================================");



var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

string queueName = "a";
string exchangeName = "alii";
await channel.QueueDeclareAsync(queue: queueName,
							durable: false,
							exclusive: false,
							autoDelete: false,
							arguments: null);

await channel.ExchangeDeclareAsync(exchange: exchangeName, ExchangeType.Direct);

await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: "al");



//Dictionary<string, object> arguments = new Dictionary<string, object>()
//{
//	{"x-match","all" },
//	{"name","ali" }
//};


var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
	var body = ea.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);
	Console.WriteLine($" [x] Received {message}");
	await channel.BasicRejectAsync(ea.DeliveryTag, false);
	//return Task.CompletedTask;
};

await channel.BasicConsumeAsync(queueName, false, consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();