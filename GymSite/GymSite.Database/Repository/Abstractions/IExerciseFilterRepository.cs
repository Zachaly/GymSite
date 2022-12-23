namespace GymSite.Database.Repository.Abstractions
{
    public interface IExerciseFilterRepository
    {
        IEnumerable<T> GetFilters<T>(Func<ExerciseFilter, T> selector);
        Task AddFilter(ExerciseFilter filter);
        Task RemoveFilter(int id);
    }
}
