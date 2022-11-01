namespace Mango.Services.Email.Messaging;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Mango.Services.Email.Messages;
using Mango.Services.Email.Repository;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqPaymentConsumer : BackgroundService
{
    private readonly EmailRepository emailRepository;
    private readonly string orderPaymentProcessQueueName;
    private readonly string exchangeName;
    private readonly string hostAddress;
    private IConnection connection;
    private IModel channel;
    private string queueName = "";

    public RabbitMqPaymentConsumer(IConfiguration configuration, EmailRepository emailRepository)
    {
        this.emailRepository = emailRepository;
        this.orderPaymentProcessQueueName = configuration.GetValue<string>("RabbitMq:OrderPaymentProcessQueueName");
        this.exchangeName = configuration.GetValue<string>("RabbitMq:ExchangeName");
        this.hostAddress = configuration.GetValue<string>("RabbitMq:HostAddress");

        var factory = new ConnectionFactory
        {
            Uri = new Uri(hostAddress)
        };

        this.connection = factory.CreateConnection();
        this.channel = connection.CreateModel();
        this.channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
        this.queueName = this.channel.QueueDeclare().QueueName;
        this.channel.QueueBind(queueName, exchangeName, "");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (channel, args) =>
        {
            var content = Encoding.UTF8.GetString(args.Body.ToArray());
            UpdatePaymentResultMessage updatePaymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(content);

            HandleMessage(updatePaymentResultMessage).GetAwaiter().GetResult();

            this.channel.BasicAck(args.DeliveryTag, false);
        };

        channel.BasicConsume(queueName, false, consumer);

        return Task.CompletedTask;
    }

    private async Task HandleMessage(UpdatePaymentResultMessage updatePaymentResultMessage)
    {
        try
        {
            await this.emailRepository.SendAndLogEmail(updatePaymentResultMessage);
        }
        catch (Exception)
        {

            throw;
        }
    }
}
