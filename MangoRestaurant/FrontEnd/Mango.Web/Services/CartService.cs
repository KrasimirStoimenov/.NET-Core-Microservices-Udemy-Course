namespace Mango.Web.Services;

using System.Net.Http;
using System.Threading.Tasks;

using Mango.Web.Models;
using Mango.Web.Models.CartModels;
using Mango.Web.Services.IServices;

public class CartService : BaseService, ICartService
{
    private readonly IHttpClientFactory httpClient;

    public CartService(IHttpClientFactory httpClient)
        : base(httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<T> GetCartByUserIdAsync<T>(string userId, string token = null)
    {
        ApiRequest apiRequest = new ApiRequest
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = StaticDetails.ShoppingCartAPIBase + $"/api/cart/{userId}",
            AccessToken = token
        };

        var response = await this.SendAsync<T>(apiRequest);

        return response;
    }

    public async Task<T> AddToCartAsync<T>(CartModel cartModel, string token = null)
    {
        ApiRequest apiRequest = new ApiRequest
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = cartModel,
            Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/add",
            AccessToken = token
        };

        var response = await this.SendAsync<T>(apiRequest);

        return response;
    }

    public async Task<T> UpdateCartAsync<T>(CartModel cartModel, string token = null)
    {
        ApiRequest apiRequest = new ApiRequest
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = cartModel,
            Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/update",
            AccessToken = token
        };

        var response = await this.SendAsync<T>(apiRequest);

        return response;
    }

    public async Task<T> RemoveFromCartAsync<T>(int cartId, string token = null)
    {
        ApiRequest apiRequest = new ApiRequest
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = cartId,
            Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/remove",
            AccessToken = token
        };

        var response = await this.SendAsync<T>(apiRequest);

        return response;
    }

    public async Task<T> ClearCartAsync<T>(string userId, string token = null)
    {
        ApiRequest apiRequest = new ApiRequest
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = userId,
            Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/clear",
            AccessToken = token
        };

        var response = await this.SendAsync<T>(apiRequest);

        return response;
    }
}
