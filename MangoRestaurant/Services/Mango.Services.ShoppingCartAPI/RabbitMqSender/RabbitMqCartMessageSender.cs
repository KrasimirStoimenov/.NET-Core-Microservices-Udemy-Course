namespace Mango.Services.ShoppingCartAPI.RabbitMqSender;

using System.Text;

using Mango.Services.ShoppingCartAPI.Messages;

using Newtonsoft.Json;

using RabbitMQ.Client;

public class RabbitMqCartMessageSender : IRabbitMqCartMessageSender
{
    private readonly string hostname;
    private readonly string username;
    private readonly string password;
    IConnection connection;

    public RabbitMqCartMessageSender(IConfiguration configuration)
    {
        this.hostname = configuration.GetValue<string>("RabbitMq:Hostname");
        this.username = configuration.GetValue<string>("RabbitMq:Username");
        this.password = configuration.GetValue<string>("RabbitMq:Password");
    }

    public void SendMessage(BaseMessage message, string queueName)
    {
        if (ConnectionExists())
        {
            using var channel = this.connection.CreateModel();
            channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }

    private void CreateConnection()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = this.hostname,
                UserName = this.username,
                Password = this.password
            };
            this.connection = factory.CreateConnection();
        }
        catch (Exception)
        {
            //log exception
        }
    }

    private bool ConnectionExists()
    {
        if (this.connection != null)
        {
            return true;
        }
        CreateConnection();
        return this.connection != null;
    }
}
