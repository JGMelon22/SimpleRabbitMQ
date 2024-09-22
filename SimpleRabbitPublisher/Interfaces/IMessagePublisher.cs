namespace SimpleRabbitPublisher.Interfaces;

public interface IMessagePublisher
{
    void SendMessage<T>(T message);
}
