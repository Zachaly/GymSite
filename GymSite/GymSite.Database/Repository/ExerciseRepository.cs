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

        public Task AddExerciseAsync(Exercise exercise)
        {
            _dbContext.Exercise.Add(exercise);

            return _dbContext.SaveChangesAsync();
        }

        public IEnumerable<T> GetDefaultExercises<T>(Func<Exercise, T> selector)
            => _dbContext.Exercise
                .Where(exercise => exercise.Default)
                .Select(selector);

        public T GetExerciseById<T>(int id, Func<Exercise, T> selector)
            => _dbContext.Exercise
                .Include(exercise => exercise.Records)
                .Where(exercise => exercise.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public IEnumerable<T> GetExercisesByUserIdWithDefaults<T>(string userId, Func<Exercise, T> selector)
            => _dbContext.Exercise
                .Where(exercise => exercise.UserId == userId || exercise.Default)
                .Select(selector);

        public Task RemoveExerciseByIdAsync(int id)
        {
            _dbContext.Exercise.Remove(_dbContext.Exercise.FirstOrDefault(ex => ex.Id == id));

            return _dbContext.SaveChangesAsync();
        }
    }
}
