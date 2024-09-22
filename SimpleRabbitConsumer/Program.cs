using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "172.17.0.3" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($" [X] Received Message: {message}");
};

channel.BasicConsume(queue: "orders",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine($" Press [enter] to exit.");
Console.ReadKey();
