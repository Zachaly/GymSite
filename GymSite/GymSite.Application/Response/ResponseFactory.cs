using GymSite.Application.Abstractions;
using GymSite.Domain.Utils;
using GymSite.Models.Response;

namespace GymSite.Application
{
    [Implementation(typeof(IResponseFactory))]
    public class ResponseFactory : IResponseFactory
    {
        public ResponseModel CreateFail(string message, Dictionary<string, IEnumerable<string>> errors)
            => new ResponseModel
            {
                Message = message,
                ValidationErrors = errors,
                Success = false
            };

        public DataResponseModel<T> CreateFail<T>(string message, Dictionary<string, IEnumerable<string>>? errors)
            => new DataResponseModel<T>
            {
                ValidationErrors = errors,
                Message = message,
                Success = false,
                Data = default(T)
            };

        public ResponseModel CreateSuccess(string message = "")
            => new ResponseModel
            {
                ValidationErrors = null,
                Message = message,
                Success = true
            };

        public DataResponseModel<T> CreateSuccess<T>(T data, string message = "")
            => new DataResponseModel<T>
            {
                Data = data,
                ValidationErrors = null,
                Message = message,
                Success = true
            };
    }
}
