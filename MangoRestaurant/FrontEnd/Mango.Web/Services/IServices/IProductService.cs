namespace Mango.Web.Services.Interfaces;

using Mango.Web.Models;

public interface IProductService
{
    Task<T> GetAllProductsAsync<T>();

    Task<T> GetProductByIdAsync<T>(int id);

    Task<T> CreateProductAsync<T>(ProductModel productDto);

    Task<T> UpdateProductAsync<T>(ProductModel productDto);

    Task<T> DeleteProductAsync<T>(int id);
}
