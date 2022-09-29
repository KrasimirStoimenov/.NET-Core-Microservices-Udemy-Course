namespace Mango.Web.Controllers;

using Mango.Web.Models;
using Mango.Web.Models.CartModels;
using Mango.Web.Models.CouponModels;
using Mango.Web.Services.IServices;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

public class CartController : Controller
{
    private readonly ICartService cartService;
    private readonly IProductService productService;
    private readonly ICouponService couponService;

    public CartController(ICartService cartService, IProductService productService, ICouponService couponService)
    {
        this.cartService = cartService;
        this.productService = productService;
        this.couponService = couponService;
    }
    public async Task<IActionResult> CartIndex()
    {
        return View(await LoadCartModelBasedOnLoggedInUser());
    }

    public async Task<IActionResult> Remove(int cartDetailsId)
    {
        string userId = this.User.Claims.Where(x => x.Type == "sub")?.FirstOrDefault()?.Value;
        string accessToken = await this.HttpContext.GetTokenAsync("access_token");

        CartModel cartModel = new CartModel();
        ResponseModel response = await this.cartService.RemoveFromCartAsync<ResponseModel>(cartDetailsId, accessToken);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(CartIndex));
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartModel cartModel)
    {
        string userId = this.User.Claims.Where(x => x.Type == "sub")?.FirstOrDefault()?.Value;
        string accessToken = await this.HttpContext.GetTokenAsync("access_token");

        ResponseModel response = await this.cartService.ApplyCoupon<ResponseModel>(cartModel, accessToken);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(CartIndex));
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RemoveCoupon(CartModel cartModel)
    {
        string userId = this.User.Claims.Where(x => x.Type == "sub")?.FirstOrDefault()?.Value;
        string accessToken = await this.HttpContext.GetTokenAsync("access_token");

        ResponseModel response = await this.cartService.RemoveCoupon<ResponseModel>(cartModel.CartHeader.UserId, accessToken);
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(CartIndex));
        }

        return View();
    }

    private async Task<CartModel> LoadCartModelBasedOnLoggedInUser()
    {
        string userId = this.User.Claims.Where(x => x.Type == "sub")?.FirstOrDefault()?.Value;
        string accessToken = await this.HttpContext.GetTokenAsync("access_token");

        CartModel cartModel = new CartModel();
        ResponseModel response = await this.cartService.GetCartByUserIdAsync<ResponseModel>(userId, accessToken);
        if (response != null && response.IsSuccess)
        {
            cartModel = JsonConvert.DeserializeObject<CartModel>(Convert.ToString(response.Result));
        }

        if (cartModel.CartHeader != null)
        {
            if (!string.IsNullOrEmpty(cartModel.CartHeader.CouponCode))
            {
                ResponseModel couponResponse = await this.couponService.GetCoupon<ResponseModel>(cartModel.CartHeader.CouponCode, accessToken);
                if (couponResponse != null && couponResponse.IsSuccess && couponResponse.Result != null)
                {
                    CouponModel coupon = JsonConvert.DeserializeObject<CouponModel>(Convert.ToString(couponResponse.Result));
                    cartModel.CartHeader.DiscountTotal = coupon.DiscountAmount;
                }
            }

            foreach (var detail in cartModel.CartDetails)
            {
                cartModel.CartHeader.OrderTotal += (detail.Product.Price * detail.Count);
            }

            cartModel.CartHeader.OrderTotal -= cartModel.CartHeader.DiscountTotal;
        }

        return cartModel;
    }
}
