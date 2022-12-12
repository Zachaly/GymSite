using GymSite.Database.Repository;
using GymSite.Domain.Entity;

namespace GymSite.Tests.Unit.Repository
{
    public class ExerciseRecordRepositoryTests : DatabaseTest
    {
        [Test]
        public async Task AddRecord()
        {
            var dbContext = CreateDbContext();
            
            var repository = new ExerciseRecordRepository(dbContext);

            var record = new ExerciseRecord
            {
                ExerciseId = 1,
                Reps = 2,
                UserId = "id",
                Weight = 3
            };

            await repository.AddRecordAsync(record);

            Assert.That(dbContext.ExerciseRecord.Any(x => 
                x.ExerciseId == record.ExerciseId &&
                x.Reps == record.Reps &&
                x.UserId == record.UserId &&
                x.Weight == record.Weight));
        }

        [Test]
        public async Task RemoveRecordById()
        {
            var records = new List<ExerciseRecord>
            {
                new ExerciseRecord
                {
                    Id = 1,
                    UserId = "id",
                    ExerciseId = 2,
                },
                new ExerciseRecord
                {
                    Id = 2,
                    UserId = "id",
                    ExerciseId = 2,
                },
                new ExerciseRecord
                {
                    Id = 3,
                    UserId = "id",
                    ExerciseId = 2,
                },
                new ExerciseRecord
                {
                    Id = 4,
                    UserId = "id",
                    ExerciseId = 2,
                },
            };

            var dbContext = CreateDbContext();
            await dbContext.AddContent(records);

            var repository = new ExerciseRecordRepository(dbContext);

            const int recordId = 3;

            await repository.RemoveRecordByIdAsync(recordId);

            Assert.That(!dbContext.ExerciseRecord.Any(x => x.Id == recordId));
        }
    }
}
