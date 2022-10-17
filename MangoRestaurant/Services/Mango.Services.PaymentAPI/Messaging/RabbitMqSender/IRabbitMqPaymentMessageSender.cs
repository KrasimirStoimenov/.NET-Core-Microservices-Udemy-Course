namespace Mango.Services.PaymentAPI.Messaging.RabbitMqSender;

using Mango.Services.PaymentAPI.Messages;

public interface IRabbitMqPaymentMessageSender
{
    void SendMessage(BaseMessage baseMessage);
}
