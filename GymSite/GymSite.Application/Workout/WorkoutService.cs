using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Utils;
using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;

namespace GymSite.Application
{
    [Implementation(typeof(IWorkoutService))]
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IWorkoutFactory _workoutFactory;
        private readonly IResponseFactory _responseFactory;

        public WorkoutService(IWorkoutRepository workoutRepository, IWorkoutFactory workoutFactory,
            IResponseFactory responseFactory)
        {
            _workoutRepository = workoutRepository;
            _workoutFactory = workoutFactory;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddWorkoutAsync(AddWorkoutRequest request)
        {
            var workout = _workoutFactory.Create(request);

            await _workoutRepository.AddWorkoutAsync(workout);

            return _responseFactory.CreateSuccess();
        }

        public async Task<ResponseModel> DeleteWorkoutByIdAsync(int id)
        {
            try
            {
                await _workoutRepository.DeleteWorkoutByIdAsync(id);
                return _responseFactory.CreateSuccess();
            }catch(Exception ex)
            {
                return _responseFactory.CreateFail(ex.Message, null);
            }
        }

        public DataResponseModel<IEnumerable<WorkoutListItemModel>> GetUserWorkouts(string userId)
        {
            var data = _workoutRepository.GetUserWorkouts(userId, workout => _workoutFactory.CreateListItem(workout));

            return _responseFactory.CreateSuccess(data);
        }

        public DataResponseModel<WorkoutModel> GetWorkoutById(int id)
        {
            try
            {
                var data = _workoutRepository.GetWorkoutById(id, workout => _workoutFactory.CreateModel(workout));

                return _responseFactory.CreateSuccess(data);
            } catch(Exception ex)
            {
                return _responseFactory.CreateFail<WorkoutModel>(ex.Message, null);
            }
        }

        public async Task<ResponseModel> UpdateWorkoutAsync(UpdateWorkoutRequest request)
        {
            try
            {
                var workout = _workoutRepository.GetWorkoutById(request.Id, x => x);

                workout.Description = request.Description ?? workout.Description;
                workout.Name = request.Name ?? workout.Name;

                await _workoutRepository.UpdateWorkoutAsync(workout);

                return _responseFactory.CreateSuccess();
            } catch (Exception ex)
            {
                return _responseFactory.CreateFail(ex.Message, null);
            }
        }
    }
}
