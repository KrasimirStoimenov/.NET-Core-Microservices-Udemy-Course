namespace Mango.Services.CouponAPI.Controllers;

using Mango.Services.CouponAPI.Models.Dtos;
using Mango.Services.CouponAPI.Repository;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/coupon")]
public class CouponController : Controller
{
    protected ResponseDto response;
    private readonly ICouponRepository couponRepository;

    public CouponController(ICouponRepository couponRepository)
    {
        this.couponRepository = couponRepository;
        this.response = new ResponseDto();
    }

    [HttpGet]
    [Route("{code}")]
    public async Task<ResponseDto> GetDiscountForCode(string code)
    {
        try
        {
            var coupon = await this.couponRepository.GetCouponByCode(code);
            response.Result = coupon;
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }
}
