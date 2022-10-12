namespace Mango.Services.Email.Repository;

using Mango.Services.Email.Messages;

public interface IEmailRepository
{
    Task SendAndLogEmail(UpdatePaymentResultMessage message);
}
