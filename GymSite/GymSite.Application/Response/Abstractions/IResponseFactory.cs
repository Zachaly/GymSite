using GymSite.Models.Response;

namespace GymSite.Application.Abstractions
{
    public interface IResponseFactory
    {
        ResponseModel CreateSuccess(string message = "");
        ResponseModel CreateFail(string message, Dictionary<string, IEnumerable<string>> errors);
        DataResponseModel<T> CreateSuccess<T>(T data, string message = "");
        DataResponseModel<T> CreateFail<T>(string message, Dictionary<string, IEnumerable<string>>? errors);
    }
}
