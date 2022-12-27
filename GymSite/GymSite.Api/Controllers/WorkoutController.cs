using GymSite.Api.Infrastructure;
using GymSite.Application.Abstractions;
using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSite.Api.Controllers
{
    [Route("api/workout")]
    [Authorize]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        /// <summary>
        /// Returns list of workout of given user
        /// </summary>
        /// <response code="200">List of workouts</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<DataResponseModel<IEnumerable<WorkoutListItemModel>>> Get([FromQuery] string? userId)
        {
            var res = _workoutService.GetUserWorkouts(userId);

            return res.CreateOkOrBadRequest();
        }

        /// <summary>
        /// Returns workout with given id
        /// </summary>
        /// <response code="200">Workout model</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<DataResponseModel<WorkoutModel>> GetById(int id)
        {
            var res = _workoutService.GetWorkoutById(id);

            return res.CreateOkOrNotFound();
        }

        /// <summary>
        /// Adds workout with given data
        /// </summary>
        /// <response code="204">Workout added successfully</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PostAsync(AddWorkoutRequest request)
        {
            var res = await _workoutService.AddWorkoutAsync(request);

            return res.CreateNoContentOrBadRequest();
        }

        /// <summary>
        /// Updates given workout
        /// </summary>
        /// <response code="204">Workout updated successfully</response>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> PutAsync(UpdateWorkoutRequest request)
        {
            var res = await _workoutService.UpdateWorkoutAsync(request);

            return res.CreateNoContentOrBadRequest();
        }

        /// <summary>
        /// Deletes workout with given id
        /// </summary>
        /// <response code="204">Workout added successfully</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync(int id)
        {
            var res = await _workoutService.DeleteWorkoutByIdAsync(id);

            return res.CreateNoContentOrBadRequest();
        }
    }
}
