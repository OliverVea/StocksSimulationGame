using Core.Messages;

namespace Messages;

public class MessageBus(Wolverine.IMessageBus messageBus) : IMessageBus
{
    public Task SendAsync<T>(T message, CancellationToken cancellationToken) where T : Message
    {
        return messageBus.SendAsync(message).AsTask();
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : Message
    {
        return messageBus.PublishAsync(message).AsTask();
    }
}