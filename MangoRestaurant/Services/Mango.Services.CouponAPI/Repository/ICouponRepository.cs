namespace Mango.Services.CouponAPI.Repository;

using Mango.Services.CouponAPI.Models.Dtos;

public interface ICouponRepository
{
    Task<CouponDto> GetCouponByCode(string couponCode);
}
