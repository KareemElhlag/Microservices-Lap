using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "rabbitmq" }; 
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "orders", durable: false, exclusive: false, autoDelete: false, arguments: null);

while (true)
{
    string message = $"New Order at {DateTime.Now.ToLongTimeString()}";
    var body = Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "orders", body: body);
    Console.WriteLine($" [x] Sent '{message}'");
    await Task.Delay(2000); 
}