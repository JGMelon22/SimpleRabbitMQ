using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using SimpleRabbitPublisher.Interfaces;

namespace SimpleRabbitPublisher.Infrastructure.Message;

public class RabbitMQPublisher : IMessagePublisher
{
    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory { HostName = "172.17.0.3" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "orders",
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: "orders",
                             basicProperties: null,
                             body: body);
    }
}
