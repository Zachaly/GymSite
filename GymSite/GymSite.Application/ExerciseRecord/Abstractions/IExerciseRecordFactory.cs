using GymSite.Domain.Entity;
using GymSite.Models.Record;
using GymSite.Models.Record.Request;

namespace GymSite.Application.Abstractions
{
    public interface IExerciseRecordFactory
    {
        ExerciseRecord Create(AddExerciseRecordRequest request);
        ExerciseRecordModel CreateModel(ExerciseRecord record);
    }
}
