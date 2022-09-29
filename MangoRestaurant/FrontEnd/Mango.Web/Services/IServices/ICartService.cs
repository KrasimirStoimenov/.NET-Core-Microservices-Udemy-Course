namespace Mango.Web.Services.IServices;

using Mango.Web.Models.CartModels;

public interface ICartService
{
    Task<T> GetCartByUserIdAsync<T>(string userId, string token = null);
    Task<T> AddToCartAsync<T>(CartModel cartModel, string token = null);
    Task<T> UpdateCartAsync<T>(CartModel cartModel, string token = null);
    Task<T> RemoveFromCartAsync<T>(int cartId, string token = null);
    Task<T> ClearCartAsync<T>(string userId, string token = null);
    Task<T> ApplyCoupon<T>(CartModel cartModel, string token = null);
    Task<T> RemoveCoupon<T>(string userId, string token = null);
}
