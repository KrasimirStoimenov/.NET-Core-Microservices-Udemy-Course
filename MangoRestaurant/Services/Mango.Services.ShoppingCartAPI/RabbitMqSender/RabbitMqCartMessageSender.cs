namespace Mango.Services.ShoppingCartAPI.RabbitMqSender;

using System.Text;

using Mango.Services.ShoppingCartAPI.Messages;

using Newtonsoft.Json;

using RabbitMQ.Client;

public class RabbitMqCartMessageSender : IRabbitMqCartMessageSender
{
    IConnection connection;

    public RabbitMqCartMessageSender() { }

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
            var factory = new ConnectionFactory();
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
