using GymSite.Api.Infrastructure;
using GymSite.Application.Abstractions;
using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSite.Api.Controllers
{
    [Route("api/workout-exercise")]
    [Authorize]
    public class WorkoutExerciseController : ControllerBase
    {
        private readonly IWorkoutExerciseService _workoutExerciseService;

        public WorkoutExerciseController(IWorkoutExerciseService workoutExerciseService)
        {
            _workoutExerciseService = workoutExerciseService;
        }

        /// <summary>
        /// Adds workout exercise with given data and returns model
        /// </summary>
        /// <response code="200">Model of created exercise</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DataResponseModel<WorkoutExerciseModel>>> PostAsync(AddWorkoutExerciseRequest request)
        {
            var res = await _workoutExerciseService.AddWorkoutExerciseAsync(request);

            return res.CreateOkOrBadRequest();
        }

        /// <summary>
        /// Deletes workout exercise with given id
        /// </summary>
        /// <response code="204">Exercise deleted successfully</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(int id)
        {
            var res = await _workoutExerciseService.DeleteWorkoutExerciseByIdAsync(id);

            return res.CreateNoContentOrBadRequest();
        }
    }
}
