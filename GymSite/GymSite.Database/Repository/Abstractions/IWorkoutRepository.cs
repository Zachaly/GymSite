namespace GymSite.Database.Repository.Abstractions
{
    public interface IWorkoutRepository
    {
        T GetWorkoutById<T>(int id, Func<Workout, T> selector);
        IEnumerable<T> GetUserWorkouts<T>(string userId, Func<Workout, T> selector);
        Task AddWorkoutAsync(Workout workout);
        Task DeleteWorkoutByIdAsync(int id);
        Task UpdateWorkoutAsync(Workout workout);
    }
}
