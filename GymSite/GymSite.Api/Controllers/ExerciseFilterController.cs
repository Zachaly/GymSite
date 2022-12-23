using GymSite.Api.Infrastructure;
using GymSite.Application.Abstractions;
using GymSite.Models.ExerciseFilter;
using GymSite.Models.ExerciseFilter.Request;
using GymSite.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSite.Api.Controllers
{
    [Route("api/exercise-filter")]
    public class ExerciseFilterController
    {
        private readonly IExerciseFilterService _exerciseFilterService;

        public ExerciseFilterController(IExerciseFilterService exerciseFilterService)
        {
            _exerciseFilterService = exerciseFilterService;
        }

        /// <summary>
        /// Returns list of exercise filters
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<DataResponseModel<IEnumerable<ExerciseFilterModel>>> Get()
        {
            var res = _exerciseFilterService.GetFilters();

            return res.CreateOkOrBadRequest();
        }

        /// <summary>
        /// Creates new filter and returns new filter model
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DataResponseModel<ExerciseFilterModel>>> Post(AddFilterRequest request)
        {
            var res = await _exerciseFilterService.AddFilter(request);

            return res.CreateOkOrBadRequest();
        }

        /// <summary>
        /// Removes filter with given id
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> Delete(int id)
        {
            var res = await _exerciseFilterService.RemoveFilter(id);

            return res.CreateNoContentOrBadRequest();
        }
    }
}
