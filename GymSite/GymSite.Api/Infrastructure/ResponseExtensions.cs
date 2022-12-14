using GymSite.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace GymSite.Api.Infrastructure
{
    public static class ResponseExtensions
    {
        public static ActionResult<ResponseModel> CreateOkOrBadRequest(this ResponseModel response)
        {
            if(response.Success)
                return new OkObjectResult(response);
            else
                return new BadRequestObjectResult(response);
        }

        public static ActionResult<ResponseModel> CreateNoContentOrBadRequest(this ResponseModel response)
        {
            if (response.Success)
                return new NoContentResult();
            else
                return new BadRequestObjectResult(response);
        }

        public static ActionResult<DataResponseModel<T>> CreateOkOrNotFound<T>(this DataResponseModel<T> response)
        {
            if(response.Data is null)
                return new NotFoundObjectResult(response);

            return new OkObjectResult(response);
        }
    }
}
