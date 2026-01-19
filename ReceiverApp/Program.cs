using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "rabbitmq" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "orders", durable: false, exclusive: false, autoDelete: false, arguments: null);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [v] Received: {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(queue: "orders", autoAck: true, consumer: consumer);
Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");
await Task.Delay(-1); 