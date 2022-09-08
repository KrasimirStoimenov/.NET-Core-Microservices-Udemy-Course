namespace Mango.Web.Services;

using System.Text;
using System.Threading.Tasks;

using Mango.Web.Models;
using Mango.Web.Services.IServices;

using Newtonsoft.Json;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory httpClient;

    public BaseService(IHttpClientFactory httpClient)
    {
        this.responseModel = new ResponseModel();
        this.httpClient = httpClient;
    }

    public ResponseModel responseModel { get; set; }

    public async Task<T> SendAsync<T>(ApiRequest apiRequest)
    {
        try
        {
            var client = httpClient.CreateClient("MangoAPI");
            HttpRequestMessage message = GetRequestMessage(apiRequest, client);

            HttpResponseMessage apiResponse = await client.SendAsync(message);

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var apiResponseModel = JsonConvert.DeserializeObject<T>(apiContent);

            return apiResponseModel;
        }
        catch (Exception ex)
        {
            ResponseModel responseModel = new ResponseModel
            {
                DisplayMessage = "Error",
                ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                IsSuccess = false
            };

            var response = JsonConvert.SerializeObject(responseModel);
            var apiResponseModel = JsonConvert.DeserializeObject<T>(response);

            return apiResponseModel;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(true);
    }

    private static HttpRequestMessage GetRequestMessage(ApiRequest apiRequest, HttpClient client)
    {
        HttpRequestMessage message = new HttpRequestMessage();
        message.Headers.Add("Accept", "application/json");
        message.RequestUri = new Uri(apiRequest.Url);
        client.DefaultRequestHeaders.Clear();
        if (apiRequest.Data != null)
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
        }

        SetHttpRequestMessageMethod(apiRequest, message);
        return message;
    }

    private static void SetHttpRequestMessageMethod(ApiRequest apiRequest, HttpRequestMessage message)
    {
        switch (apiRequest.ApiType)
        {
            case StaticDetails.ApiType.POST:
                message.Method = HttpMethod.Post;
                break;
            case StaticDetails.ApiType.PUT:
                message.Method = HttpMethod.Put;
                break;
            case StaticDetails.ApiType.DELETE:
                message.Method = HttpMethod.Delete;
                break;
            default:
                message.Method = HttpMethod.Get;
                break;
        }
    }
}
