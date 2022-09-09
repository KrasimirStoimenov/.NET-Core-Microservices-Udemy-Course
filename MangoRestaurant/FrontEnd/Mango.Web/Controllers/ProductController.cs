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
            var response = await this.productService.CreateProductAsync<ResponseModel>(model);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        return View(model);
    }

    public async Task<IActionResult> EditProduct(int productId)
    {
        var response = await this.productService.GetProductByIdAsync<ResponseModel>(productId);
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
            var response = await this.productService.UpdateProductAsync<ResponseModel>(model);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        return View(model);
    }

    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var response = await this.productService.GetProductByIdAsync<ResponseModel>(productId);
        if (response != null && response.IsSuccess)
        {
            ProductModel product = JsonConvert.DeserializeObject<ProductModel>(Convert.ToString(response.Result));

            return View(product);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(ProductModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await this.productService.DeleteProductAsync<ResponseModel>(model.ProductId);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        return View(model);
    }
}
