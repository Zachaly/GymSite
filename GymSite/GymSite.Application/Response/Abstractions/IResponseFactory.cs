using GymSite.Models;
using GymSite.Models.Response;

namespace GymSite.Application.Response.Abstractions
{
    public interface IResponseFactory
    {
        ResponseModel CreateSuccess(ResponseCode code, string message);
        ResponseModel CreateFail(ResponseCode code, string message, Dictionary<string, IEnumerable<string>> errors);
        DataResponseModel<T> CreateSuccess<T>(ResponseCode code, string message, T data);
        DataResponseModel<T> CreateFail<T>(ResponseCode code, string message, Dictionary<string, IEnumerable<string>>? errors);
    }
}
