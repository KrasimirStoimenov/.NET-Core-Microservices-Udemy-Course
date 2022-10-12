namespace Mango.Services.Email.DbContext;

using Mango.Services.Email.Models;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<EmailLog> EmailLogs { get; set; }
}
