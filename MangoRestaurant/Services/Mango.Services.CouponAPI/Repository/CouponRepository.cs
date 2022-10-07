namespace Mango.Services.CouponAPI.Repository;

using System.Threading.Tasks;

using AutoMapper;

using Mango.Services.CouponAPI.DbContext;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dtos;

using Microsoft.EntityFrameworkCore;

public class CouponRepository : ICouponRepository
{
    private readonly ApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public CouponRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<CouponDto> GetCouponByCode(string couponCode)
    {
        Coupon couponFromDb = await this.dbContext.Coupons.FirstOrDefaultAsync(x => x.CouponCode == couponCode);

        CouponDto coupon = this.mapper.Map<CouponDto>(couponFromDb);

        return coupon;
    }
}
