using GymSite.Database.Repository.Abstractions;

namespace GymSite.Database.Repository
{
    [Implementation(typeof(IExerciseRecordRepository))]
    public class ExerciseRecordRepository : IExerciseRecordRepository
    {
        private ApplicationDbContext _dbContext;

        public ExerciseRecordRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddRecordAsync(ExerciseRecord record)
        {
            _dbContext.ExerciseRecord.Add(record);

            return _dbContext.SaveChangesAsync();
        }

        public Task RemoveRecordByIdAsync(int id)
        {
            _dbContext.ExerciseRecord.Remove(_dbContext.ExerciseRecord.FirstOrDefault(x => x.Id == id));

            return _dbContext.SaveChangesAsync();
        }
    }
}
