namespace Mango.Services.ShoppingCartAPI.Controllers;

using Mango.Services.ShoppingCartAPI.Messages;
using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Repository;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
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
    [Route("add")]
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
    [Route("update")]
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
    [Route("remove")]
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
    [Route("clear")]
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

    [HttpPost]
    [Route("applyCoupon")]
    public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cartDto)
    {
        try
        {
            bool isSuccess = await this.cartRepository.ApplyCoupon(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode); ;
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
    [Route("removeCoupon")]
    public async Task<ResponseDto> RemoveCoupon([FromBody] string userId)
    {
        try
        {
            bool isSuccess = await this.cartRepository.RemoveCoupon(userId);
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
    [Route("checkout")]
    public async Task<object> Checkout(ChekoutHeaderDto checkoutHeader)
    {
        try
        {
            CartDto cartDto = await this.cartRepository.GetCartByUserId(checkoutHeader.UserId);
            if (cartDto == null)
            {
                return BadRequest();
            }

            checkoutHeader.CartDetails = cartDto.CartDetails;

            //logic to add message to process order.
        }
        catch (Exception ex)
        {
            this.response.IsSuccess = false;
            this.response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return response;
    }
}
