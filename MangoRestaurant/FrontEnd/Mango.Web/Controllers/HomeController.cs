namespace Mango.Web.Controllers;
using System.Diagnostics;

using Mango.Web.Models;
using Mango.Web.Services.IServices;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly IProductService productService;

    public HomeController(ILogger<HomeController> logger, IProductService productService)
    {
        this.logger = logger;
        this.productService = productService;
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
