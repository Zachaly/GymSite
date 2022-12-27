using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;

namespace GymSite.Application.Abstractions
{
    public interface IWorkoutExerciseService
    {
        Task<DataResponseModel<WorkoutExerciseModel>> AddWorkoutExerciseAsync(AddWorkoutExerciseRequest request);
        Task<ResponseModel> DeleteWorkoutExerciseByIdAsync(int id);
    }
}
