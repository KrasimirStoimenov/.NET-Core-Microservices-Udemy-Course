namespace Mango.Services.ShoppingCartAPI.Repository;

using Mango.Services.ShoppingCartAPI.Models.Dtos;

public interface ICouponRepository
{
    Task<CouponDto> GetCoupon(string couponName);
}
