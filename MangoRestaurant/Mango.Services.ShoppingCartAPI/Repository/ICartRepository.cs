namespace Mango.Services.ShoppingCartAPI.Repository;

using Mango.Services.ShoppingCartAPI.Models.Dtos;

public interface ICartRepository
{
    Task<CartDto> GetCartByUserId(string userId);

    Task<CartDto> CreateUpdateCart(CartDto cartDto);

    Task<bool> RemoveFromCart(int cartDetailsId);

    Task<bool> ClearCart(string userId);

    Task<bool> ApplyCoupon(string userId, string couponCode);

    Task<bool> RemoveCoupon(string userId);
}
