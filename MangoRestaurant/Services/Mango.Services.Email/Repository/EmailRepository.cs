namespace Mango.Services.Email.Repository;

using System.Threading.Tasks;

using Mango.Services.Email.DbContext;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Models;

using Microsoft.EntityFrameworkCore;

public class EmailRepository : IEmailRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbContext;

    public EmailRepository(DbContextOptions<ApplicationDbContext> dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task SendAndLogEmail(UpdatePaymentResultMessage message)
    {
        // implement an email sender or call some other class library

        EmailLog emailLog = new EmailLog()
        {
            Email = message.Email,
            EmailSent = DateTime.Now,
            Log = $"Order - {message.OrderId} has been created successfully"
        };

        await using var database = new ApplicationDbContext(this.dbContext);
        database.EmailLogs.Add(emailLog);
        await database.SaveChangesAsync();
    }
}
