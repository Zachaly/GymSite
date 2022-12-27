using GymSite.Database.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GymSite.Database.Repository
{
    [Implementation(typeof(IWorkoutExerciseRepository))]
    public class WorkoutExerciseRepository : IWorkoutExerciseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public WorkoutExerciseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddWorkoutExerciseAsync(WorkoutExercise workoutExercise)
        {
            if(!_dbContext.Exercise.Any(exercise => exercise.Id == workoutExercise.ExerciseId))
            {
                throw new Exception("Exercise does not exist");
            }

            _dbContext.WorkoutExercise.Add(workoutExercise);

            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteWorkoutExerciseAsync(int id)
        {
            var exercise = _dbContext.WorkoutExercise.FirstOrDefault(x => x.Id == id);
            _dbContext.WorkoutExercise.Remove(exercise);

            return _dbContext.SaveChangesAsync();
        }

        public T GetWorkoutExerciseById<T>(int id, Func<WorkoutExercise, T> selector)
            => _dbContext.WorkoutExercise
                .Include(exercise => exercise.Exercise)
                .Where(exercise => exercise.Id == id)
                .Select(selector)
                .FirstOrDefault();
    }
}
