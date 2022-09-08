namespace Mango.Web.Services.IServices;

using Mango.Web.Models;

public interface IBaseService : IDisposable
{
    ResponseModel responseModel { get; set; }

    Task<T> SendAsync<T>(ApiRequest apiRequest);
}
