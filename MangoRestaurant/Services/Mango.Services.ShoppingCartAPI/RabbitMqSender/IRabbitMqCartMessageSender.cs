namespace Mango.Services.ShoppingCartAPI.RabbitMqSender;

using Mango.Services.ShoppingCartAPI.Messages;

public interface IRabbitMqCartMessageSender
{
    void SendMessage(BaseMessage baseMessage, string queueName);
}
