using GymSite.Database.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GymSite.Database.Repository
{
    [Implementation(typeof(IExerciseSetRepository))]
    public class ExerciseSetRepository : IExerciseSetRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ExerciseSetRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddExerciseSetAsync(ExerciseSet exerciseSet)
        {
            _dbContext.ExerciseSet.Add(exerciseSet);

            return _dbContext.SaveChangesAsync();
        }

        public Task RemoveExerciseSetByIdAsync(int id)
        {
            var set = _dbContext.ExerciseSet.FirstOrDefault(x => x.Id == id);

            _dbContext.ExerciseSet.Remove(set);

            return _dbContext.SaveChangesAsync();
        }
    }
}
