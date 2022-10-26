namespace Mango.Services.OrderAPI.Messaging;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Mango.Services.OrderAPI.Messages;
using Mango.Services.OrderAPI.Messaging.RabbitMqSender;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqCheckoutConsumer : BackgroundService
{
    private readonly IRabbitMqOrderMessageSender rabbitMqOrderMessageSender;
    private readonly OrderRepository orderRepository;
    private readonly string hostAddress;
    private readonly string checkoutQueueName;
    private readonly string orderPaymentProcessQueueName;
    private IConnection connection;
    private IModel channel;

    public RabbitMqCheckoutConsumer(
        IConfiguration configuration,
        OrderRepository orderRepository,
        IRabbitMqOrderMessageSender rabbitMqOrderMessageSender)
    {
        this.orderRepository = orderRepository;
        this.rabbitMqOrderMessageSender = rabbitMqOrderMessageSender;

        this.checkoutQueueName = configuration.GetValue<string>("RabbitMq:CheckoutQueueName");
        this.orderPaymentProcessQueueName = configuration.GetValue<string>("RabbitMq:OrderPaymentProcessQueueName");
        this.hostAddress = configuration.GetValue<string>("RabbitMq:HostAddress");
        var factory = new ConnectionFactory
        {
            Uri = new Uri(hostAddress)
        };

        this.connection = factory.CreateConnection();
        this.channel = this.connection.CreateModel();
        this.channel.QueueDeclare(queue: this.checkoutQueueName, false, false, false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(this.channel);
        consumer.Received += (channel, args) =>
        {
            var content = Encoding.UTF8.GetString(args.Body.ToArray());
            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(content);

            HandleMessage(checkoutHeaderDto).GetAwaiter().GetResult();

            this.channel.BasicAck(args.DeliveryTag, false);
        };

        this.channel.BasicConsume(this.checkoutQueueName, false, consumer);

        return Task.CompletedTask;
    }

    private async Task HandleMessage(CheckoutHeaderDto checkoutHeaderDto)
    {
        OrderHeader orderHeader = new OrderHeader()
        {
            UserId = checkoutHeaderDto.UserId,
            FirstName = checkoutHeaderDto.FirstName,
            LastName = checkoutHeaderDto.LastName,
            OrderDetails = new List<OrderDetails>(),
            CardNumber = checkoutHeaderDto.CardNumber,
            CouponCode = checkoutHeaderDto.CouponCode,
            CVV = checkoutHeaderDto.CVV,
            DiscountTotal = checkoutHeaderDto.DiscountTotal,
            Email = checkoutHeaderDto.Email,
            ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
            OrderTime = DateTime.Now,
            OrderTotal = checkoutHeaderDto.OrderTotal,
            PaymentStatus = false,
            Phone = checkoutHeaderDto.Phone,
            PickupDateTime = checkoutHeaderDto.PickupDateTime,
        };

        foreach (var detailList in checkoutHeaderDto.CartDetails)
        {
            OrderDetails orderDetails = new OrderDetails()
            {
                ProductId = detailList.ProductId,
                ProductName = detailList.Product.Name,
                Price = detailList.Product.Price,
                Count = detailList.Count,
            };

            orderHeader.CartTotalItems += detailList.Count;
            orderHeader.OrderDetails.Add(orderDetails);
        }

        await this.orderRepository.AddOrder(orderHeader);

        PaymentRequestMessage paymentRequestMessage = new PaymentRequestMessage()
        {
            Name = $"{orderHeader.FirstName} {orderHeader.LastName}",
            CardNumber = orderHeader.CardNumber,
            CVV = orderHeader.CVV,
            ExpiryMonthYear = orderHeader.ExpiryMonthYear,
            OrderId = orderHeader.OrderHeaderId,
            OrderTotal = orderHeader.OrderTotal,
            Email = orderHeader.Email,
        };

        try
        {
            this.rabbitMqOrderMessageSender.SendMessage(paymentRequestMessage, this.orderPaymentProcessQueueName);
        }
        catch (Exception)
        {

            throw;
        }
    }
}
