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

            var filterIds = new List<int> { 1, 2, 3 };

            var repository = new ExerciseRepository(dbContext);

            await repository.AddExerciseAsync(exercise, filterIds);

            Assert.Multiple(() =>
            {
                Assert.That(dbContext.Exercise.Contains(exercise));
                Assert.That(dbContext.ExerciseExerciseFilter.Any(x => filterIds.Contains(x.FilterId)));
            });
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
            dbContext.AddContent(new List<ExerciseRecord> { record, new ExerciseRecord { Exercise = testExercise, UserId = "id2" } });

            var repository = new ExerciseRepository(dbContext);

            var res = repository.GetExerciseById(testExercise.Id, record.UserId, x => x);

            Assert.Multiple(() =>
            {
                Assert.That(res.Name, Is.EqualTo(testExercise.Name));
                Assert.That(res.Description, Is.EqualTo(testExercise.Description));
                Assert.That(res.Records, Is.Not.Null);
                Assert.That(res.Records.All(x => x.UserId == record.UserId));
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
                new Exercise
                {
                    Id = 5,
                    Description = "desc3",
                    Name = "name3",
                    UserId = "id",
                    Default = true
                },
            };

            var dbContext = CreateDbContext();
            dbContext.AddContent(exercises);

            var repository = new ExerciseRepository(dbContext);

            const string UserId = "id";

            var res = repository.GetExercisesByUserIdWithDefaults(UserId, x => x);

            Assert.Multiple(() =>
            {
                Assert.That(res.All(x => x.UserId == UserId));
                Assert.That(res, Is.EquivalentTo(exercises.Where(x => x.UserId == UserId || x.Default)));
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

        [Test]
        public void GetDefaultExercises()
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
                    UserId = "id2",
                    Default = true
                },
                new Exercise
                {
                    Id = 4,
                    Description = "desc3",
                    Name = "name3",
                    UserId = "id"
                },
                new Exercise
                {
                    Id = 5,
                    Description = "desc3",
                    Name = "name3",
                    UserId = "id",
                    Default = true
                },
            };

            var dbContext = CreateDbContext();
            dbContext.AddContent(exercises);

            var repository = new ExerciseRepository(dbContext);


            var res = repository.GetDefaultExercises(x => x);

            Assert.That(res, Is.EquivalentTo(exercises.Where(x => x.Default)));
        }

        [Test]
        [TestCase(new[] { 11, 12 })]
        [TestCase(new[] { 11 })]
        [TestCase(new[] { 31 })]
        public async Task GetExercisesWithFilter(int[] filterIds)
        {
            var exercises = new List<Exercise>
            {
                new Exercise
                {
                    Id = 2,
                    Description = "desc",
                    Name = "name",
                    UserId = "id",
                    ExerciseFilters = new List<ExerciseExerciseFilter>
                    {
                        new ExerciseExerciseFilter { ExerciseId = 2, FilterId = 10 },
                        new ExerciseExerciseFilter { ExerciseId = 2, FilterId = 11 },
                        new ExerciseExerciseFilter { ExerciseId = 2, FilterId = 12 }
                    }
                },
                new Exercise
                {
                    Id = 3,
                    Description = "desc2",
                    Name = "name2",
                    UserId = "id",
                    ExerciseFilters = new List<ExerciseExerciseFilter>
                    {
                        new ExerciseExerciseFilter { ExerciseId = 3, FilterId = 20 },
                        new ExerciseExerciseFilter { ExerciseId = 3, FilterId = 21 },
                        new ExerciseExerciseFilter { ExerciseId = 3, FilterId = 12 }
                    }
                },
                new Exercise
                {
                    Id = 4,
                    Description = "desc3",
                    Name = "name3",
                    UserId = "id",
                    ExerciseFilters = new List<ExerciseExerciseFilter>
                    {
                        new ExerciseExerciseFilter { ExerciseId = 4, FilterId = 15 },
                        new ExerciseExerciseFilter { ExerciseId = 4, FilterId = 11 },
                        new ExerciseExerciseFilter { ExerciseId = 4, FilterId = 13 }
                    }
                },
                new Exercise
                {
                    Id = 5,
                    Description = "desc3",
                    Name = "name3",
                    UserId = "id",
                    ExerciseFilters = new List<ExerciseExerciseFilter>
                    {
                        new ExerciseExerciseFilter { ExerciseId = 5, FilterId = 31 },
                        new ExerciseExerciseFilter { ExerciseId = 5, FilterId = 33 },
                        new ExerciseExerciseFilter { ExerciseId = 5, FilterId = 32 }
                    }
                },
            };

            var dbContext = CreateDbContext();

            await dbContext.AddContent(exercises);


            var repository = new ExerciseRepository(dbContext);

            var res = repository.GetExercisesWithFilter("id", filterIds, x => x).ToList();

            var test = res.SelectMany(x => x.ExerciseFilters).ToList();

            Assert.That(res.SelectMany(x => x.ExerciseFilters).Any(x => filterIds.Contains(x.FilterId)));
        }
    }
}
