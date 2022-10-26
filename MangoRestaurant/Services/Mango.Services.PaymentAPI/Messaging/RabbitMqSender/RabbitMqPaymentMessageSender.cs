﻿namespace Mango.Services.PaymentAPI.Messaging.RabbitMqSender;

using System.Text;

using Mango.Services.PaymentAPI.Messages;

using Newtonsoft.Json;

using RabbitMQ.Client;

public class RabbitMqPaymentMessageSender : IRabbitMqPaymentMessageSender
{
    private readonly string exchangeName;
    IConnection connection;

    public RabbitMqPaymentMessageSender(IConfiguration configuration)
    {
        this.exchangeName = configuration.GetValue<string>("RabbitMq:ExchangeName");
    }

    public void SendMessage(BaseMessage message)
    {
        if (ConnectionExists())
        {
            using var channel = this.connection.CreateModel();
            channel.ExchangeDeclare(this.exchangeName, ExchangeType.Fanout, durable: false);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: this.exchangeName, "", basicProperties: null, body: body);
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
