namespace GymSite.Database.Repository.Abstractions
{
    public interface IWorkoutExerciseRepository
    {
        Task AddWorkoutExerciseAsync(WorkoutExercise workoutExercise);
        Task DeleteWorkoutExerciseAsync(int id);
        T GetWorkoutExerciseById<T>(int id, Func<WorkoutExercise, T> selector);
    }
}
