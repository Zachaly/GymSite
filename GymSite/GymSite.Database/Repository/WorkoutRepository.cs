using GymSite.Database.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GymSite.Database.Repository
{
    [Implementation(typeof(IWorkoutRepository))]
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public WorkoutRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddWorkoutAsync(Workout workout)
        {
            _dbContext.Workout.Add(workout);

            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteWorkoutByIdAsync(int id)
        {
            var workout = _dbContext.Workout.FirstOrDefault(x => x.Id == id);
            _dbContext.Workout.Remove(workout);

            return _dbContext.SaveChangesAsync();
        }

        public IEnumerable<T> GetUserWorkouts<T>(string userId, Func<Workout, T> selector)
        => _dbContext.Workout
            .Where(workout => workout.UserId == userId)
            .Select(selector);

        public T GetWorkoutById<T>(int id, Func<Workout, T> selector)
        {
            var workout = _dbContext.Workout.Include(x => x.User).FirstOrDefault(x => x.Id == id);

            workout.Exercises = _dbContext.WorkoutExercise
                .Include(exercise => exercise.Exercise)
                .Include(exercise => exercise.ExerciseSets)
                .Where(exercise => exercise.WorkoutId == id)
                .ToList();

            return selector(workout);
        }

        public Task UpdateWorkoutAsync(Workout workout)
        {
            _dbContext.Workout.Update(workout);

            return _dbContext.SaveChangesAsync();
        }
    }
}
