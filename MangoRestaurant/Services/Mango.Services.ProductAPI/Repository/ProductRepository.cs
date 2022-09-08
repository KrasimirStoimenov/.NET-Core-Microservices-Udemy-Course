namespace Mango.Services.ProductAPI.Repository;

using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Mango.Services.ProductAPI.DbContext;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos;

using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public ProductRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        List<Product> products = await this.dbContext.Products.ToListAsync();

        List<ProductDto> productsDto = this.mapper.Map<List<ProductDto>>(products);

        return productsDto;
    }

    public async Task<ProductDto> GetProductById(int productId)
    {
        Product product = await this.dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);

        ProductDto productDto = this.mapper.Map<ProductDto>(product);

        return productDto;
    }

    public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
    {
        Product product = this.mapper.Map<Product>(productDto);

        if (product.ProductId > 0)
        {
            this.dbContext.Update(product);
        }
        else
        {
            this.dbContext.Add(product);

        }

        await this.dbContext.SaveChangesAsync();

        return this.mapper.Map<ProductDto>(product);
    }

    public async Task<bool> DeleteProduct(int productId)
    {
        try
        {
            Product product = await this.dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);

            if (product == null)
            {
                return false;
            }

            this.dbContext.Remove(product);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
