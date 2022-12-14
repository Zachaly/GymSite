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

        /// <summary>
        /// Creates an exercise record based on data in request
        /// </summary>
        /// <response code="204">Record added successfully</response>
        /// <response code="404">Data in request is not viable</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResponseModel>> Post(AddExerciseRecordRequest request)
        {
            var res = await _exerciseRecordService.AddRecord(request);

            return res.CreateNoContentOrBadRequest();
        }

        /// <summary>
        /// Removes exercise record with specified id
        /// </summary>
        /// <param name="id">Record id</param>
        /// <response code="204">Record removed successfully</response>
        /// <response code="404">Error occured during removal process</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResponseModel>> Delete(int id)
        {
            var res = await _exerciseRecordService.RemoveRecord(id);

            return res.CreateNoContentOrBadRequest();
        }
    }
}
