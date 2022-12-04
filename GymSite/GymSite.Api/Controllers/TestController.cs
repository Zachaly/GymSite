using GymSite.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GymSite.Api.Controllers
{
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test() => Ok(new DataResponseModel<string> { Data = "test" });
    }
}
