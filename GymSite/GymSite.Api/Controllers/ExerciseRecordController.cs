using GymSite.Api.Infrastructure;
using GymSite.Application.Abstractions;
using GymSite.Models.Record.Request;
using GymSite.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace GymSite.Api.Controllers
{
    [Route("api/exercise-record")]
    public class ExerciseRecordController : ControllerBase
    {
        private readonly IExerciseRecordService _exerciseRecordService;

        public ExerciseRecordController(IExerciseRecordService exerciseRecordService)
        {
            _exerciseRecordService = exerciseRecordService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel>> Post(AddExerciseRecordRequest request)
        {
            var res = await _exerciseRecordService.AddRecord(request);

            return res.CreateNoContentOrBadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel>> Delete(int id)
        {
            var res = await _exerciseRecordService.RemoveRecord(id);

            return res.CreateNoContentOrBadRequest();
        }
    }
}
