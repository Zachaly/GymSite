using GymSite.Api.Infrastructure;
using GymSite.Application.Abstractions;
using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSite.Api.Controllers
{
    [Route("api/exercise-set")]
    [Authorize]
    public class ExerciseSetController : ControllerBase
    {
        private readonly IExerciseSetService _exerciseSetService;

        public ExerciseSetController(IExerciseSetService exerciseSetService)
        {
            _exerciseSetService = exerciseSetService;
        }

        /// <summary>
        /// Adds exercise set with given data and returns model
        /// </summary>
        /// <response code="200">Model of created set</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DataResponseModel<ExerciseSetModel>>> PostAsync(AddExerciseSetRequest request)
        {
            var res = await _exerciseSetService.AddExerciseSetAsync(request);

            return res.CreateOkOrBadRequest();
        }

        /// <summary>
        /// Deletes exercise set with given id
        /// </summary>
        /// <response code="204">Set deleted successfully</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(int id)
        {
            var res = await _exerciseSetService.DeleteExerciseSetByIdAsync(id);

            return res.CreateNoContentOrBadRequest();
        }
    }
}
