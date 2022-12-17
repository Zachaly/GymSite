namespace GymSite.Database.Repository.Abstractions
{
    public interface IExerciseRepository
    {
        Task AddExerciseAsync(Exercise exercise);
        T GetExerciseById<T>(int id, Func<Exercise, T> selector);
        IEnumerable<T> GetExercisesByUserIdWithDefaults<T>(string userId, Func<Exercise, T> selector);
        Task RemoveExerciseByIdAsync(int id);
        IEnumerable<T> GetDefaultExercises<T>(Func<Exercise, T> selector);
    }
}
