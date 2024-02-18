namespace Core.Messages;

public interface IMessageBus
{
    Task SendAsync<T>(T message, CancellationToken cancellationToken) where T : Message;
    Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : Message;
}