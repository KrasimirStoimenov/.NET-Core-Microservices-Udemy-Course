namespace Mango.Services.CouponAPI.DbContext;

using Mango.Services.CouponAPI.Models;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<Coupon> Coupons { get; set; }
}
