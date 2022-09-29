namespace Mango.Web.Services;

using System.Threading.Tasks;

using Mango.Web.Models;
using Mango.Web.Services.IServices;

public class CouponService : BaseService, ICouponService
{
    private readonly IHttpClientFactory httpClient;

    public CouponService(IHttpClientFactory httpClient)
        : base(httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<T> GetCoupon<T>(string couponCode, string token = null)
    {
        ApiRequest apiRequest = new ApiRequest
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = StaticDetails.CouponAPIBase + $"/api/coupon/{couponCode}",
            AccessToken = token
        };

        var response = await this.SendAsync<T>(apiRequest);

        return response;
    }
}
