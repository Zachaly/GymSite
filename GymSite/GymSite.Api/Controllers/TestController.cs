using GymSite.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace GymSite.Api.Controllers
{
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Test endpoint to check ability to send requests to api
        /// </summary>
        [HttpGet]
        public IActionResult Test() => Ok(new DataResponseModel<string> { Data = "test" });
    }
}
