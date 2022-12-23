using GymSite.Database.Repository.Abstractions;

namespace GymSite.Database.Repository
{
    [Implementation(typeof(IExerciseFilterRepository))]
    public class ExerciseFilterRepository : IExerciseFilterRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ExerciseFilterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddFilter(ExerciseFilter filter)
        {
            _dbContext.ExerciseFilter.Add(filter);

            return _dbContext.SaveChangesAsync();
        }

        public IEnumerable<T> GetFilters<T>(Func<ExerciseFilter, T> selector)
            => _dbContext.ExerciseFilter.Select(selector);

        public Task RemoveFilter(int id)
        {
            var filter = _dbContext.ExerciseFilter.FirstOrDefault(filter => filter.Id == id);

            _dbContext.ExerciseFilter.Remove(filter);

            return _dbContext.SaveChangesAsync();
        }
    }
}
