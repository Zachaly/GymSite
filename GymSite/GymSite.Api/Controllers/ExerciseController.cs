using GymSite.Api.Infrastructure;
using GymSite.Application.Abstractions;
using GymSite.Models.Exercise.Request;
using GymSite.Models.Response;
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

        [HttpGet("user/{userId}")]
        public ActionResult<ResponseModel> GetUserExercices(string userId)
        {
            var res = _exerciseService.GetUserExercises(userId);

            return res.CreateOkOrBadRequest();
        }

        [HttpGet("{id}")]
        public ActionResult<ResponseModel> GetExercise(int id)
        {
            var res = _exerciseService.GetExerciseById(id);

            return res.CreateOkOrBadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel>> AddExercise(AddExerciseRequest request)
        {
            var res = await _exerciseService.AddExercise(request);

            return res.CreateNoContentOrBadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel>> RemoveExercise(int id)
        {
            var res = await _exerciseService.RemoveExercise(id);

            return res.CreateNoContentOrBadRequest();
        }
    }
}
