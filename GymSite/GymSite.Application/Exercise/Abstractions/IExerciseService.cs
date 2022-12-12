using GymSite.Models.Exercise;
using GymSite.Models.Exercise.Request;
using GymSite.Models.Response;

namespace GymSite.Application.Abstractions
{
    public interface IExerciseService
    {
        DataResponseModel<ExerciseModel> GetExerciseById(int id);
        DataResponseModel<IEnumerable<ExerciseListItemModel>> GetUserExercises(string userId);
        Task<ResponseModel> AddExercise(AddExerciseRequest request);
        Task<ResponseModel> RemoveExercise(int id);
    }
}
