using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;

namespace GymSite.Application.Abstractions
{
    public interface IWorkoutService
    {
        DataResponseModel<IEnumerable<WorkoutListItemModel>> GetUserWorkouts(string userId);
        DataResponseModel<WorkoutModel> GetWorkoutById(int id);
        Task<ResponseModel> AddWorkoutAsync(AddWorkoutRequest request);
        Task<ResponseModel> UpdateWorkoutAsync(UpdateWorkoutRequest request);
        Task<ResponseModel> DeleteWorkoutByIdAsync(int id);
    }
}
