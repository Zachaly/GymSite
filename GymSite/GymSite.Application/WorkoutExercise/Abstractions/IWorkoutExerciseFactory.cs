using GymSite.Domain.Entity;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;

namespace GymSite.Application.Abstractions
{
    public interface IWorkoutExerciseFactory
    {
        WorkoutExercise Create(AddWorkoutExerciseRequest request);
        WorkoutExerciseModel CreateModel(WorkoutExercise workoutExercise);
    }
}