using GymSite.Database.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GymSite.Database.Repository
{
    [Implementation(typeof(IExerciseRepository))]
    public class ExerciseRepository : IExerciseRepository
    {
        private ApplicationDbContext _dbContext;

        public ExerciseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddExerciseAsync(Exercise exercise, IEnumerable<int> filterIds)
        {
            _dbContext.Exercise.Add(exercise);
            await _dbContext.SaveChangesAsync();
            if (filterIds.Any())
            {
                _dbContext.ExerciseExerciseFilter.AddRange(filterIds.Select(x => new ExerciseExerciseFilter
                {
                    ExerciseId = exercise.Id,
                    FilterId = x
                }));
            }

            await _dbContext.SaveChangesAsync();
        }

        public IEnumerable<T> GetDefaultExercises<T>(Func<Exercise, T> selector)
            => _dbContext.Exercise
                .Where(exercise => exercise.Default)
                .Select(selector);

        public T GetExerciseById<T>(int exerciseId, string? userId, Func<Exercise, T> selector)
        {
            var exercise = _dbContext.Exercise
                .Include(exercise => exercise.ExerciseFilters)
                .ThenInclude(filter => filter.Filter)
                .FirstOrDefault(exercise => exercise.Id == exerciseId);

            exercise.Records = _dbContext.ExerciseRecord
                .Where(record => record.ExerciseId == exerciseId && record.UserId == userId)
                .ToList();

            return selector(exercise);
        }
            

        public IEnumerable<T> GetExercisesByUserIdWithDefaults<T>(string userId, Func<Exercise, T> selector)
            => _dbContext.Exercise
                .Where(exercise => exercise.UserId == userId || exercise.Default)
                .Select(selector);

        public IEnumerable<T> GetExercisesWithFilter<T>(string userId, IEnumerable<int> filterIds, Func<Exercise, T> selector)
            => _dbContext.Exercise
                .Include(exercise => exercise.ExerciseFilters)
                .AsEnumerable()
                .Where(exercise => exercise.ExerciseFilters.Count() > 0 &&
                    filterIds.All(id => exercise.ExerciseFilters.Any(filter => filter.FilterId == id))
                    && (exercise.UserId == userId || exercise.Default))
                .Select(selector);

        public Task RemoveExerciseByIdAsync(int id)
        {
            _dbContext.Exercise.Remove(_dbContext.Exercise.FirstOrDefault(ex => ex.Id == id));

            return _dbContext.SaveChangesAsync();
        }
    }
}
