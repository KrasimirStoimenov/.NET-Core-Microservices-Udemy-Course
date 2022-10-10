namespace Mango.Services.ShoppingCartAPI.Repository;

using System.Threading.Tasks;

using Mango.Services.ShoppingCartAPI.Models.Dtos;

using Newtonsoft.Json;

public class CouponRepository : ICouponRepository
{
    private readonly HttpClient httpClient;

    public CouponRepository(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<CouponDto> GetCoupon(string couponName)
    {
        var response = await this.httpClient.GetAsync($"/api/coupon/{couponName}");
        var apiContent = await response.Content.ReadAsStringAsync();

        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        if (resp.IsSuccess)
        {
            return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
        }

        return new CouponDto();
    }
}
