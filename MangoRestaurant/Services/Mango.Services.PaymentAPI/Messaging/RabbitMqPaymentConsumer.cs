namespace Mango.Services.PaymentAPI.Messaging;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Mango.Services.PaymentAPI.Messages;
using Mango.Services.PaymentAPI.Messaging.RabbitMqSender;

using Newtonsoft.Json;

using PaymentProcessor;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqPaymentConsumer : BackgroundService
{
    private readonly IRabbitMqPaymentMessageSender rabbitMqPaymentMessageSender;
    private readonly IProcessPayment processPayment;
    private readonly string orderPaymentProcessQueueName;
    private IConnection connection;
    private IModel channel;

    public RabbitMqPaymentConsumer(
        IConfiguration configuration,
        IRabbitMqPaymentMessageSender rabbitMqOrderMessageSender,
        IProcessPayment processPayment)
    {
        this.rabbitMqPaymentMessageSender = rabbitMqOrderMessageSender;
        this.processPayment = processPayment;
        this.orderPaymentProcessQueueName = configuration.GetValue<string>("RabbitMq:OrderPaymentProcessQueueName");

        var factory = new ConnectionFactory
        {
            HostName = configuration.GetValue<string>("RabbitMq:Hostname"),
            UserName = configuration.GetValue<string>("RabbitMq:Username"),
            Password = configuration.GetValue<string>("RabbitMq:Password"),
        };

        this.connection = factory.CreateConnection();
        this.channel = this.connection.CreateModel();
        this.channel.QueueDeclare(queue: this.orderPaymentProcessQueueName, false, false, false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(this.channel);
        consumer.Received += (channel, args) =>
        {
            var content = Encoding.UTF8.GetString(args.Body.ToArray());
            PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(content);

            HandleMessage(paymentRequestMessage).GetAwaiter().GetResult();

            this.channel.BasicAck(args.DeliveryTag, false);
        };

        this.channel.BasicConsume(this.orderPaymentProcessQueueName, false, consumer);

        return Task.CompletedTask;
    }

    private async Task HandleMessage(PaymentRequestMessage paymentRequestMessage)
    {
        var result = this.processPayment.PaymentProcessor();

        UpdatePaymentResultMessage updatePaymentResultMessage = new UpdatePaymentResultMessage()
        {
            Status = result,
            OrderId = paymentRequestMessage.OrderId,
            Email = paymentRequestMessage.Email
        };

        try
        {
            this.rabbitMqPaymentMessageSender.SendMessage(updatePaymentResultMessage);
        }
        catch (Exception)
        {

            throw;
        }
    }
}
