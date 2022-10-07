namespace Mango.Services.OrderAPI.Repository;

using System.Threading.Tasks;

using Mango.Services.OrderAPI.DbContext;
using Mango.Services.OrderAPI.Models;

using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbContext;

    public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<bool> AddOrder(OrderHeader orderHeader)
    {
        await using var database = new ApplicationDbContext(this.dbContext);

        database.OrderHeaders.Add(orderHeader);
        await database.SaveChangesAsync();

        return true;
    }

    public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
    {
        await using var database = new ApplicationDbContext(this.dbContext);

        var orderHeaderFromDb = await database.OrderHeaders.FirstOrDefaultAsync(x => x.OrderHeaderId == orderHeaderId);
        if (orderHeaderFromDb != null)
        {
            orderHeaderFromDb.PaymentStatus = paid;
            await database.SaveChangesAsync();
        }
    }
}
