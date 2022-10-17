namespace Mango.Services.OrderAPI.Messaging.RabbitMqSender;

using Mango.Services.OrderAPI.Messages;

public interface IRabbitMqOrderMessageSender
{
    void SendMessage(BaseMessage baseMessage, string queueName);
}
