using GymSite.Database.Repository;
using GymSite.Domain.Entity;

namespace GymSite.Tests.Unit.Repository
{
    [TestFixture]
    public class ExerciseRepositoryTests : DatabaseTest
    {
        [Test]
        public async Task AddExercise()
        {
            var dbContext = CreateDbContext();

            var exercise = new Exercise
            {
                Description = "desc",
                Name = "name",
            };

            var repository = new ExerciseRepository(dbContext);

            await repository.AddExerciseAsync(exercise);

            Assert.That(dbContext.Exercise.Contains(exercise));
        }

        [Test]
        public void GetExerciseById()
        {
            var testExercise = new Exercise
            {
                Id = 2,
                Description = "desc",
                Name = "name",
            };

            var exercises = new List<Exercise>
            {
                testExercise,
                new Exercise
                {
                    Id = 3,
                    Description = "desc2",
                    Name = "name2",
                },
                new Exercise
                {
                    Id = 4,
                    Description = "desc3",
                    Name = "name3",
                },
            };

            var record = new ExerciseRecord { Exercise = testExercise, UserId = "id" };

            var dbContext = CreateDbContext();
            dbContext.AddContent(exercises);
            dbContext.AddContent(new List<ExerciseRecord> { record });

            var repository = new ExerciseRepository(dbContext);

            var res = repository.GetExerciseById(testExercise.Id, x => x);

            Assert.Multiple(() =>
            {
                Assert.That(res.Name, Is.EqualTo(testExercise.Name));
                Assert.That(res.Description, Is.EqualTo(testExercise.Description));
                Assert.That(res.Records, Is.Not.Null);
            });
        }

        [Test]
        public void GetExercisesByUserId()
        {
            var exercises = new List<Exercise>
            {
                new Exercise
                {
                    Id = 2,
                    Description = "desc",
                    Name = "name",
                    UserId = "id"
                },
                new Exercise
                {
                    Id = 3,
                    Description = "desc2",
                    Name = "name2",
                    UserId = "id2"
                },
                new Exercise
                {
                    Id = 4,
                    Description = "desc3",
                    Name = "name3",
                    UserId = "id"
                },
            };

            var dbContext = CreateDbContext();
            dbContext.AddContent(exercises);

            var repository = new ExerciseRepository(dbContext);

            const string UserId = "id";

            var res = repository.GetExercisesByUserId(UserId, x => x);

            Assert.Multiple(() =>
            {
                Assert.That(res.All(x => x.UserId == UserId));
                Assert.That(res, Is.EquivalentTo(exercises.Where(x => x.UserId == UserId)));
            });
        }

        [Test]
        public async Task RemoveExerciseById()
        {
            var exercises = new List<Exercise>
            {
                new Exercise
                {
                    Id = 2,
                    Description = "desc",
                    Name = "name",
                    UserId = "id"
                },
                new Exercise
                {
                    Id = 3,
                    Description = "desc2",
                    Name = "name2",
                    UserId = "id2"
                },
                new Exercise
                {
                    Id = 4,
                    Description = "desc3",
                    Name = "name3",
                    UserId = "id"
                },
            };

            var dbContext = CreateDbContext();
            await dbContext.AddContent(exercises);

            var repository = new ExerciseRepository(dbContext);

            const int ExerciseId = 3;

            await repository.RemoveExerciseByIdAsync(ExerciseId);

            Assert.That(!dbContext.Exercise.Any(x => x.Id == ExerciseId));
        }
    }
}
