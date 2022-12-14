using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Utils;
using GymSite.Models.Exercise;
using GymSite.Models.Exercise.Request;
using GymSite.Models.Response;

namespace GymSite.Application
{
    [Implementation(typeof(IExerciseService))]
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly IExerciseFactory _exerciseFactory;

        public ExerciseService(IExerciseRepository exerciseRepository, 
            IResponseFactory responseFactory, 
            IExerciseFactory exerciseFactory)
        {
            _exerciseRepository = exerciseRepository;
            _responseFactory = responseFactory;
            _exerciseFactory = exerciseFactory;
        }

        public async Task<ResponseModel> AddExercise(AddExerciseRequest request)
        {
            var exercise = _exerciseFactory.Create(request);

            await _exerciseRepository.AddExerciseAsync(exercise);

            return _responseFactory.CreateSuccess();
        }

        public DataResponseModel<ExerciseModel> GetExerciseById(int id)
        {
            var data = _exerciseRepository.GetExerciseById(id, exercise => _exerciseFactory.CreateModel(exercise));

            return _responseFactory.CreateSuccess(data);
        }

        public DataResponseModel<IEnumerable<ExerciseListItemModel>> GetUserExercises(string userId)
        {
            var data = _exerciseRepository.GetExercisesByUserId(userId, exercise => _exerciseFactory.CreateListItem(exercise));

            return _responseFactory.CreateSuccess(data);
        }

        public async Task<ResponseModel> RemoveExercise(int id)
        {
            try
            {
                await _exerciseRepository.RemoveExerciseByIdAsync(id);
                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFail(ex.Message, null);
            }
        }
    }
}
