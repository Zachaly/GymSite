using GymSite.Application.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;

namespace GymSite.Application
{
    [Implementation(typeof(IWorkoutFactory))]
    public class WorkoutFactory : IWorkoutFactory
    {
        public Workout Create(AddWorkoutRequest request)
            => new Workout
            {
                Description = request.Description,
                Name = request.Name,
                UserId = request.UserId,
            };

        public WorkoutListItemModel CreateListItem(Workout workout)
            => new WorkoutListItemModel
            {
                Id = workout.Id,
                Name = workout.Name,
            };

        public WorkoutModel CreateModel(Workout workout)
            => new WorkoutModel
            {
                Id = workout.Id,
                Description = workout.Description,
                Name = workout.Name,
                Exercices = workout.Exercises.Select(exercise => new WorkoutExerciseModel
                {
                    ExerciseDescription = exercise.Exercise.Description,
                    ExerciseId = exercise.ExerciseId,
                    ExerciseName = exercise.Exercise.Name,
                    Id = exercise.Id,
                    Sets = exercise.ExerciseSets.Select(set => new ExerciseSetModel
                    {
                        Id = set.Id,
                        Reps = set.Reps,
                        Weight = set.Weigth.ToString()
                    }).ToList(),
                }).ToList(),
            };
    }
}
