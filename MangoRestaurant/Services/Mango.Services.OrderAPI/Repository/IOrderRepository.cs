namespace Mango.Services.OrderAPI.Repository;

using Mango.Services.OrderAPI.Models;

public interface IOrderRepository
{
    Task<bool> AddOrder(OrderHeader orderHeader);
    Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid);
}
