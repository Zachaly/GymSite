using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;

namespace GymSite.Application.Abstractions
{
    public interface IExerciseSetService
    {
        Task<DataResponseModel<ExerciseSetModel>> AddExerciseSetAsync(AddExerciseSetRequest request);
        Task<ResponseModel> DeleteExerciseSetByIdAsync(int id);
    }
}
