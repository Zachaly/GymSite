using GymSite.Application.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;

namespace GymSite.Application
{
    [Implementation(typeof(IWorkoutExerciseFactory))]
    public class WorkoutExerciseFactory : IWorkoutExerciseFactory
    {
        public WorkoutExercise Create(AddWorkoutExerciseRequest request)
            => new WorkoutExercise
            {
                ExerciseId = request.ExerciseId,
                WorkoutId = request.WorkoutId,
            };

        public WorkoutExerciseModel CreateModel(WorkoutExercise workoutExercise)
            => new WorkoutExerciseModel
            {
                ExerciseDescription = workoutExercise.Exercise.Description,
                ExerciseId = workoutExercise.ExerciseId,
                ExerciseName = workoutExercise.Exercise.Name,
                Id = workoutExercise.Id,
                Sets = workoutExercise.ExerciseSets?.Select(set => new ExerciseSetModel
                {
                    Reps = set.Reps,
                    Id = set.Id,
                    Weight = set.Weigth.ToString()
                }) ?? new List<ExerciseSetModel>()
            };
    }
}
