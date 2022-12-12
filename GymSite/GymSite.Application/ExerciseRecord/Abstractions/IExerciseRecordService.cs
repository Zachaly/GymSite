using GymSite.Models.Record.Request;
using GymSite.Models.Response;

namespace GymSite.Application.Abstractions
{
    public interface IExerciseRecordService
    {
        Task<ResponseModel> AddRecord(AddExerciseRecordRequest request);
        Task<ResponseModel> RemoveRecord(int id);
    }
}
