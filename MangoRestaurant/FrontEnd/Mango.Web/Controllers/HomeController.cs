namespace Mango.Web.Controllers;

using System.Diagnostics;

using Mango.Web.Models;
using Mango.Web.Models.CartModels;
using Mango.Web.Services.IServices;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly IProductService productService;
    private readonly ICartService cartService;

    public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
    {
        this.logger = logger;
        this.productService = productService;
        this.cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        List<ProductModel> products = new();
        var response = await this.productService.GetAllProductsAsync<ResponseModel>("");
        if (response != null && response.IsSuccess)
        {
            products = JsonConvert.DeserializeObject<List<ProductModel>>(Convert.ToString(response.Result));
        }

        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Details(int productId)
    {
        ProductModel model = new();
        var response = await this.productService.GetProductByIdAsync<ResponseModel>(productId, "");
        if (response != null && response.IsSuccess)
        {
            model = JsonConvert.DeserializeObject<ProductModel>(Convert.ToString(response.Result));
        }

        return View(model);
    }

    [HttpPost]
    [ActionName("Details")]
    [Authorize]
    public async Task<IActionResult> DetailsPost(ProductModel productModel)
    {
        CartModel cartModel = new CartModel()
        {
            CartHeader = new CartHeaderModel()
            {
                UserId = this.User.Claims.Where(x => x.Type == "sub")?.FirstOrDefault()?.Value
            }
        };

        CartDetailsModel cartDetails = new CartDetailsModel()
        {
            Count = productModel.Count,
            ProductId = productModel.ProductId
        };

        var response = await this.productService.GetProductByIdAsync<ResponseModel>(productModel.ProductId, "");
        if (response != null && response.IsSuccess)
        {
            cartDetails.Product = JsonConvert.DeserializeObject<ProductModel>(Convert.ToString(response.Result));
        }

        List<CartDetailsModel> cartDetailsModels = new List<CartDetailsModel>();
        cartDetailsModels.Add(cartDetails);
        cartModel.CartDetails = cartDetailsModels;

        var accessToken = await this.HttpContext.GetTokenAsync("access_token");
        var addToCartResponse = await this.cartService.AddToCartAsync<ResponseModel>(cartModel, accessToken);
        if (addToCartResponse != null && addToCartResponse.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(productModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }
}
