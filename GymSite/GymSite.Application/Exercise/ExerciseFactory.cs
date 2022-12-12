using GymSite.Application.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using GymSite.Models.Exercise;
using GymSite.Models.Exercise.Request;
using GymSite.Models.Record;

namespace GymSite.Application
{
    [Implementation(typeof(IExerciseFactory))]
    public class ExerciseFactory : IExerciseFactory
    {
        public Exercise Create(AddExerciseRequest request)
            => new Exercise
            {
                Description = request.Description,
                Name = request.Name,
                UserId = request.UserId,
            };

        public ExerciseListItemModel CreateListItem(Exercise exercise)
            => new ExerciseListItemModel
            {
                Id = exercise.Id,
                Name = exercise.Name,
            };

        public ExerciseModel CreateModel(Exercise exercise)
            => new ExerciseModel 
            { 
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                Records = exercise.Records.Select(record => new ExerciseRecordModel
                {
                    Id = record.Id,
                    Reps = record.Reps,
                    Weight = record.Weight.ToString()
                }).ToList()
            };
    }
}
