namespace Mango.Web.Controllers;

using Mango.Web.Models;
using Mango.Web.Services.IServices;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        var accessToken = await this.HttpContext.GetTokenAsync("access_token");
        var response = await this.productService.GetAllProductsAsync<ResponseModel>(accessToken);
        if (response != null && response.IsSuccess)
        {
            products = JsonConvert.DeserializeObject<List<ProductModel>>(Convert.ToString(response.Result));
        }

        return View(products);
    }

    public async Task<IActionResult> CreateProduct()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(ProductModel model)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await this.HttpContext.GetTokenAsync("access_token");
            var response = await this.productService.CreateProductAsync<ResponseModel>(model, accessToken);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        return View(model);
    }

    public async Task<IActionResult> EditProduct(int productId)
    {
        var accessToken = await this.HttpContext.GetTokenAsync("access_token");
        var response = await this.productService.GetProductByIdAsync<ResponseModel>(productId, accessToken);
        if (response != null && response.IsSuccess)
        {
            ProductModel product = JsonConvert.DeserializeObject<ProductModel>(Convert.ToString(response.Result));

            return View(product);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(ProductModel model)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await this.HttpContext.GetTokenAsync("access_token");
            var response = await this.productService.UpdateProductAsync<ResponseModel>(model, accessToken);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        return View(model);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var accessToken = await this.HttpContext.GetTokenAsync("access_token");
        var response = await this.productService.GetProductByIdAsync<ResponseModel>(productId, accessToken);
        if (response != null && response.IsSuccess)
        {
            ProductModel product = JsonConvert.DeserializeObject<ProductModel>(Convert.ToString(response.Result));

            return View(product);
        }

        return NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(ProductModel model)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await this.HttpContext.GetTokenAsync("access_token");
            var response = await this.productService.DeleteProductAsync<ResponseModel>(model.ProductId, accessToken);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        return View(model);
    }
}
