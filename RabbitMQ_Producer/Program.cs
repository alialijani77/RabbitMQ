// See https://aka.ms/new-console-template for more information

using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

IBasicProperties basicProperties = new BasicProperties();

basicProperties.Headers = new Dictionary<string, object?>()
	{
		{ "name","ali" }
	};

string exchangeName = "ali";

await channel.ExchangeDeclareAsync(exchange: exchangeName, ExchangeType.Headers);

const string message = "Hello World!";
var body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: exchangeName, routingKey: "hello", body: body);

Console.WriteLine("Hello, World!");

Console.ReadLine();
