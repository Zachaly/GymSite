using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Utils;
using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;

namespace GymSite.Application
{
    [Implementation(typeof(IExerciseSetService))]
    public class ExerciseSetService : IExerciseSetService
    {
        private readonly IExerciseSetRepository _exerciseSetRepository;
        private readonly IExerciseSetFactory _exerciseSetFactory;
        private readonly IResponseFactory _responseFactory;

        public ExerciseSetService(IExerciseSetRepository exerciseSetRepository, IExerciseSetFactory exerciseSetFactory,
            IResponseFactory responseFactory)
        {
            _exerciseSetRepository = exerciseSetRepository;
            _exerciseSetFactory = exerciseSetFactory;
            _responseFactory = responseFactory;
        }

        public async Task<DataResponseModel<ExerciseSetModel>> AddExerciseSetAsync(AddExerciseSetRequest request)
        {
            var set = _exerciseSetFactory.Create(request);

            await _exerciseSetRepository.AddExerciseSetAsync(set);

            return _responseFactory.CreateSuccess(_exerciseSetFactory.CreateModel(set));
        }

        public async Task<ResponseModel> DeleteExerciseSetByIdAsync(int id)
        {
            try
            {
                await _exerciseSetRepository.RemoveExerciseSetByIdAsync(id);
                return _responseFactory.CreateSuccess();
            } catch (Exception ex)
            {
                return _responseFactory.CreateFail(ex.Message, null);
            }
        }
    }
}
