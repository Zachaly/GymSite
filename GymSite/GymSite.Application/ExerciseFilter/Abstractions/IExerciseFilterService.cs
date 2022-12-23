using GymSite.Models.ExerciseFilter;
using GymSite.Models.ExerciseFilter.Request;
using GymSite.Models.Response;

namespace GymSite.Application.Abstractions
{
    public interface IExerciseFilterService
    {
        Task<DataResponseModel<ExerciseFilterModel>> AddFilter(AddFilterRequest request);
        Task<ResponseModel> RemoveFilter(int id);
        DataResponseModel<IEnumerable<ExerciseFilterModel>> GetFilters();
    }
}
