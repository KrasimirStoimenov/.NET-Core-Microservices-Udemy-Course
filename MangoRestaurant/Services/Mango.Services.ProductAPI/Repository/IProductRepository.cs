namespace Mango.Services.ProductAPI.Repository;

using Mango.Services.ProductAPI.Models.Dtos;

public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetProducts();

    Task<ProductDto> GetProductById(int productId);

    Task<ProductDto> CreateUpdateProduct(ProductDto productDto);

    Task<bool> DeleteProduct(int productId);
}
