namespace Mango.Web.Services;

using System.Threading.Tasks;

using Mango.Web.Models;
using Mango.Web.Services.IServices;

public class ProductService : BaseService, IProductService
{
    private readonly IHttpClientFactory httpClient;

    public ProductService(IHttpClientFactory httpClient)
        : base(httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<T> CreateProductAsync<T>(ProductModel productDto, string token)
    {
        ApiRequest apiRequest = new ApiRequest
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = productDto,
            Url = StaticDetails.ProductAPIBase + "/api/products",
            AccessToken = token
        };

        var response = await this.SendAsync<T>(apiRequest);

        return response;
    }

    public async Task<T> DeleteProductAsync<T>(int id, string token)
    {
        ApiRequest apiRequest = new ApiRequest
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = StaticDetails.ProductAPIBase + $"/api/products/{id}",
            AccessToken = token
        };

        var response = await this.SendAsync<T>(apiRequest);

        return response;
    }

    public async Task<T> GetAllProductsAsync<T>(string token)
    {
        ApiRequest apiRequest = new ApiRequest
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = StaticDetails.ProductAPIBase + "/api/products",
            AccessToken = token
        };

        var response = await this.SendAsync<T>(apiRequest);

        return response;
    }

    public async Task<T> GetProductByIdAsync<T>(int id, string token)
    {
        ApiRequest apiRequest = new ApiRequest
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = StaticDetails.ProductAPIBase + $"/api/products/{id}",
            AccessToken = token
        };

        var response = await this.SendAsync<T>(apiRequest);

        return response;
    }

    public async Task<T> UpdateProductAsync<T>(ProductModel productDto, string token)
    {
        ApiRequest apiRequest = new ApiRequest
        {
            ApiType = StaticDetails.ApiType.PUT,
            Data = productDto,
            Url = StaticDetails.ProductAPIBase + "/api/products",
            AccessToken = token
        };

        var response = await this.SendAsync<T>(apiRequest);

        return response;
    }
}
