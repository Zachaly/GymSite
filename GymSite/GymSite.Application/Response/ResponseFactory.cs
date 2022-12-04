using GymSite.Application.Response.Abstractions;
using GymSite.Domain.Utils;
using GymSite.Models.Response;
using System.Runtime.InteropServices;

namespace GymSite.Application.Response
{
    [Implementation(typeof(IResponseFactory))]
    public class ResponseFactory : IResponseFactory
    {
        public ResponseModel CreateFail(ResponseCode code, string message, Dictionary<string, IEnumerable<string>> errors)
            => new ResponseModel
            {
                Code = code,
                Message = message,
                Errors = errors,
                Success = false
            };


        public DataResponseModel<T> CreateFail<T>(ResponseCode code, string message, Dictionary<string, IEnumerable<string>>? errors)
            => new DataResponseModel<T>
            {
                Code = code,
                Errors = errors,
                Message = message,
                Success = false,
                Data = default(T)
            };

        public ResponseModel CreateSuccess(ResponseCode code, string message)
            => new ResponseModel
            {
                Code = code,
                Errors = null,
                Message = message,
                Success = true
            };

        public DataResponseModel<T> CreateSuccess<T>(ResponseCode code, string message, T data)
            => new DataResponseModel<T>
            {
                Code = code,
                Data = data,
                Errors = null,
                Message = message,
                Success = true
            };
    }
}
