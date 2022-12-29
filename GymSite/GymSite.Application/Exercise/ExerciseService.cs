using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Utils;
using GymSite.Models.Exercise;
using GymSite.Models.Exercise.Request;
using GymSite.Models.Exercise.Validator;
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

        public async Task<ResponseModel> AddDefaultExercise(AddExerciseRequest request)
        {
            var validation = new AddExerciseValidator().Validate(request);

            if(!validation.IsValid)
            {
                return _responseFactory.CreateFail("", validation.GetValidationErrors());
            }

            var exercise = _exerciseFactory.CreateDefault(request);

            await _exerciseRepository.AddExerciseAsync(exercise, request.FilterIds);

            return _responseFactory.CreateSuccess();
        }

        public async Task<ResponseModel> AddExercise(AddExerciseRequest request)
        {
            var validation = new AddExerciseValidator().Validate(request);

            if (!validation.IsValid)
            {
                return _responseFactory.CreateFail("", validation.GetValidationErrors());
            }

            var exercise = _exerciseFactory.Create(request);

            await _exerciseRepository.AddExerciseAsync(exercise, request.FilterIds ?? new int[] { });

            return _responseFactory.CreateSuccess();
        }

        public DataResponseModel<IEnumerable<ExerciseListItemModel>> GetDefaultExercises()
        {
            var data = _exerciseRepository.GetDefaultExercises(exercise => _exerciseFactory.CreateListItem(exercise));

            return _responseFactory.CreateSuccess(data);
        }

        public DataResponseModel<ExerciseModel> GetExerciseById(int id, string? userId)
        {
            try
            {
                var data = _exerciseRepository.GetExerciseById(id, userId, exercise => _exerciseFactory.CreateModel(exercise));
                return _responseFactory.CreateSuccess(data);
            } catch (Exception ex)
            {
                return _responseFactory.CreateFail<ExerciseModel>(ex.Message, null);
            }
        }

        public DataResponseModel<IEnumerable<ExerciseListItemModel>> GetExercisesWithFilter(GetExerciseRequest request)
        {
            var data = _exerciseRepository.GetExercisesWithFilter(request.UserId, request.FilterIds ?? new int[] { }, 
                exercise => _exerciseFactory.CreateListItem(exercise));

            return _responseFactory.CreateSuccess(data);
        }

        public DataResponseModel<IEnumerable<ExerciseListItemModel>> GetUserExercises(string userId)
        {
            var data = _exerciseRepository.GetExercisesByUserIdWithDefaults(userId, exercise => _exerciseFactory.CreateListItem(exercise));

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
