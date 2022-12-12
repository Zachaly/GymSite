using GymSite.Application.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using GymSite.Models.Record;
using GymSite.Models.Record.Request;

namespace GymSite.Application
{
    [Implementation(typeof(IExerciseRecordFactory))]
    public class ExerciseRecordFactory : IExerciseRecordFactory
    {
        public ExerciseRecord Create(AddExerciseRecordRequest request)
            => new ExerciseRecord
            {
                ExerciseId = request.ExerciseId,
                Reps = request.Reps,
                UserId = request.UserId,
                Weight = request.Weight,
            };

        public ExerciseRecordModel CreateModel(ExerciseRecord record)
            => new ExerciseRecordModel
            {
                Id = record.Id,
                Reps = record.Reps,
                Weight = record.Weight.ToString(),
            };
    }
}
