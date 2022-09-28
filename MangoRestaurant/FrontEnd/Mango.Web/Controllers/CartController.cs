namespace Mango.Web.Controllers;

using Mango.Web.Models;
using Mango.Web.Models.CartModels;
using Mango.Web.Services.IServices;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

public class CartController : Controller
{
    private readonly ICartService cartService;
    private readonly IProductService productService;

    public CartController(ICartService cartService, IProductService productService)
    {
        this.cartService = cartService;
        this.productService = productService;
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
            foreach (var detail in cartModel.CartDetails)
            {
                cartModel.CartHeader.OrderTotal += (detail.Product.Price * detail.Count);
            }
        }

        return cartModel;
    }
}
