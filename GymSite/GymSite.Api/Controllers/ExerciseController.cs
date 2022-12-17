using GymSite.Api.Infrastructure;
using GymSite.Application.Abstractions;
using GymSite.Models.Exercise;
using GymSite.Models.Exercise.Request;
using GymSite.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSite.Api.Controllers
{
    [Route("api/exercise")]
    public class ExerciseController
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        /// <summary>
        /// Returns exercise list of given user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <response code="200">List of user exercises</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(200)]
        [Authorize]
        public ActionResult<DataResponseModel<IEnumerable<ExerciseListItemModel>>> GetUserExercices(string userId)
        {
            var res = _exerciseService.GetUserExercises(userId);

            return res.CreateOkOrBadRequest();
        }

        /// <summary>
        /// Returns exercise specified by id
        /// </summary>
        /// <param name="id">Exercise id</param>
        /// <response code="200">Exercise model</response>
        /// <response code="404">Exercise with given id does not exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Authorize]
        public ActionResult<DataResponseModel<ExerciseModel>> GetExercise(int id)
        {
            var res = _exerciseService.GetExerciseById(id);

            return res.CreateOkOrNotFound();
        }

        /// <summary>
        /// Adds exercise with data specified in request
        /// </summary>
        /// <response code="204">Exercise added successfully</response>
        /// <response code="400">Failed to add exercise</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<ActionResult<ResponseModel>> AddExercise(AddExerciseRequest request)
        {
            var res = await _exerciseService.AddExercise(request);

            return res.CreateNoContentOrBadRequest();
        }

        /// <summary>
        /// Removes exercise with specified id
        /// </summary>
        /// <param name="id">Exercise id</param>
        /// <response code="204">Exercise removed successfully</response>
        /// <response code="400">Failed to remove exercise</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<ActionResult<ResponseModel>> RemoveExercise(int id)
        {
            var res = await _exerciseService.RemoveExercise(id);

            return res.CreateNoContentOrBadRequest();
        }

        /// <summary>
        /// Returns list of default exercises
        /// </summary>
        /// <response code="200">List of user exercises</response>
        [HttpGet("default")]
        [ProducesResponseType(200)]
        [Authorize]
        public ActionResult<DataResponseModel<IEnumerable<ExerciseListItemModel>>> GetDefault()
        {
            var res = _exerciseService.GetDefaultExercises();

            return res.CreateOkOrBadRequest();
        }

        /// <summary>
        /// Adds default exercise with data specified in request
        /// </summary>
        /// <response code="204">Exercise added successfully</response>
        /// <response code="400">Failed to add exercise</response>
        [HttpPost("default")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<ActionResult<ResponseModel>> AddDefault(AddExerciseRequest request)
        {
            var res = await _exerciseService.AddDefaultExercise(request);

            return res.CreateOkOrBadRequest();
        }
    }
}
