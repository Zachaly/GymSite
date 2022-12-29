using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Utils;
using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;
using GymSite.Models.Workout.Validator;

namespace GymSite.Application
{
    [Implementation(typeof(IWorkoutExerciseService))]
    public class WorkoutExerciseService : IWorkoutExerciseService
    {
        private readonly IWorkoutExerciseRepository _workoutExerciseRepository;
        private readonly IWorkoutExerciseFactory _workoutExerciseFactory;
        private readonly IResponseFactory _responseFactory;

        public WorkoutExerciseService(IWorkoutExerciseRepository workoutExerciseRepository, IWorkoutExerciseFactory workoutExerciseFactory,
            IResponseFactory responseFactory)
        {
            _workoutExerciseRepository = workoutExerciseRepository;
            _workoutExerciseFactory = workoutExerciseFactory;
            _responseFactory = responseFactory;
        }

        public async Task<DataResponseModel<WorkoutExerciseModel>> AddWorkoutExerciseAsync(AddWorkoutExerciseRequest request)
        {
            var validation = new AddWorkoutExerciseValidator().Validate(request);

            if(!validation.IsValid)
            {
                return _responseFactory.CreateFail<WorkoutExerciseModel>("", validation.GetValidationErrors());
            }

            try
            {
                var exercise = _workoutExerciseFactory.Create(request);

                await _workoutExerciseRepository.AddWorkoutExerciseAsync(exercise);

                return _responseFactory.CreateSuccess(
                    _workoutExerciseFactory.CreateModel(_workoutExerciseRepository.GetWorkoutExerciseById(exercise.Id, x => x)));
            } catch (Exception ex)
            {
                return _responseFactory.CreateFail<WorkoutExerciseModel>(ex.Message, null);
            }
        }

        public async Task<ResponseModel> DeleteWorkoutExerciseByIdAsync(int id)
        {
            try
            {
                await _workoutExerciseRepository.DeleteWorkoutExerciseAsync(id); 
                return _responseFactory.CreateSuccess();
            } catch (Exception ex)
            {
                return _responseFactory.CreateFail(ex.Message, null);
            }
        }
    }
}
