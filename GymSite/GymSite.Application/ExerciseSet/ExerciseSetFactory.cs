using GymSite.Application.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;

namespace GymSite.Application
{
    [Implementation(typeof(IExerciseSetFactory))]
    public class ExerciseSetFactory : IExerciseSetFactory
    {
        public ExerciseSet Create(AddExerciseSetRequest request)
            => new ExerciseSet
            {
                ExerciseId = request.WorkoutExerciseId,
                Reps = request.Reps,
                Weigth = request.Weight,
            };

        public ExerciseSetModel CreateModel(ExerciseSet exerciseSet)
            => new ExerciseSetModel
            {
                Id = exerciseSet.Id,
                Reps = exerciseSet.Reps,
                Weight = exerciseSet.Weigth.ToString(),
            };
    }
}
