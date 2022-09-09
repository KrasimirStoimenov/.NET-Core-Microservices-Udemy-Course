namespace Mango.Web.Controllers;

using Mango.Web.Models;
using Mango.Web.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

public class ProductController : Controller
{
    private readonly IProductService productService;

    public ProductController(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<IActionResult> ProductIndex()
    {
        List<ProductModel> products = new List<ProductModel>();

        var response = await this.productService.GetAllProductsAsync<ResponseModel>();
        if (response != null && response.IsSuccess)
        {
            products = JsonConvert.DeserializeObject<List<ProductModel>>(Convert.ToString(response.Result));
        }

        return View(products);
    }
}
