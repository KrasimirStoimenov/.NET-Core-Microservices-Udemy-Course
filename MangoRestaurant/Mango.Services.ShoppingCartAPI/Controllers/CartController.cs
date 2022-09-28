namespace Mango.Services.ShoppingCartAPI.Controllers;

using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Repository;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cart")]
public class CartController
{
    protected ResponseDto response;
    private readonly ICartRepository cartRepository;

    public CartController(ICartRepository cartRepository)
    {
        this.cartRepository = cartRepository;
        this.response = new ResponseDto();
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<ResponseDto> GetCart(string userId)
    {
        try
        {
            CartDto cartDto = await this.cartRepository.GetCartByUserId(userId);
            response.Result = cartDto;
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }

    [HttpPost]
    public async Task<ResponseDto> AddCart(CartDto cartDto)
    {
        try
        {
            CartDto cart = await this.cartRepository.CreateUpdateCart(cartDto);
            response.Result = cart;
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }

    [HttpPost]
    public async Task<ResponseDto> UpdateCart(CartDto cartDto)
    {
        try
        {
            CartDto cart = await this.cartRepository.CreateUpdateCart(cartDto);
            response.Result = cart;
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }

    [HttpPost]
    public async Task<ResponseDto> RemoveCart([FromBody] int cartId)
    {
        try
        {
            bool isSuccess = await this.cartRepository.RemoveFromCart(cartId);
            response.Result = isSuccess;
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }

    [HttpPost]
    public async Task<ResponseDto> ClearCart([FromBody] string userId)
    {
        try
        {
            bool isSuccess = await this.cartRepository.ClearCart(userId);
            response.Result = isSuccess;
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }
}
