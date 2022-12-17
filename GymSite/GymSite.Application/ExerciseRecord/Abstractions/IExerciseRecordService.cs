using GymSite.Models.Record;
using GymSite.Models.Record.Request;
using GymSite.Models.Response;

namespace GymSite.Application.Abstractions
{
    public interface IExerciseRecordService
    {
        Task<DataResponseModel<ExerciseRecordModel>> AddRecord(AddExerciseRecordRequest request);
        Task<ResponseModel> RemoveRecord(int id);
    }
}
