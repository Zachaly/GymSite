using GymSite.Database.Repository;
using GymSite.Domain.Entity;

namespace GymSite.Tests.Unit.Repository
{
    [TestFixture]
    public class WorkoutRepositoryTests : DatabaseTest
    {
        [Test]
        public async Task GetWorkoutsByUserId()
        {
            var workouts = new List<Workout>
            {
                new Workout
                {
                    Description = "desc",
                    Name = "name",
                    UserId = "id",
                },
                new Workout
                {
                    Description = "desc",
                    Name = "name",
                    UserId = "id",
                },
                new Workout
                {
                    Description = "desc",
                    Name = "name",
                    UserId = "id2",
                }
            };
            var dbContext = CreateDbContext();

            await dbContext.AddContent(workouts);

            var repository = new WorkoutRepository(dbContext);

            const string Id = "id";

            var res = repository.GetUserWorkouts(Id, x => x);

            Assert.That(res, Is.EquivalentTo(workouts.Where(x => x.UserId == Id)));
        }

        [Test]
        public async Task GetWorkoutById()
        {
            var workout = new Workout
            {
                Id = 1,
                Description = "desc",
                Name = "name",
                User = new ApplicationUser
                {
                    Id = "id2",
                    UserName = "user",
                    NickName = "nick"
                },
                Exercises = new List<WorkoutExercise>
                {
                    new WorkoutExercise
                    {
                        Exercise = new Exercise { Name = "name", Id = 1 },
                        ExerciseSets = new List<ExerciseSet>
                        {
                            new ExerciseSet { ExerciseId = 1, Reps = 2, Weigth = 3 }
                        }
                    }
                }
            };

            var dbContext = CreateDbContext();

            await dbContext.AddContent(new List<Workout> { workout });

            var repository = new WorkoutRepository(dbContext);

            var res = repository.GetWorkoutById(1, x => x);

            Assert.Multiple(() =>
            {
                Assert.That(res.Id, Is.EqualTo(1));
                Assert.That(res.Name, Is.EqualTo(workout.Name));
                Assert.That(res.Exercises, Is.Not.Null);
                Assert.That(res.Exercises.First().Exercise, Is.Not.Null);
                Assert.That(res.Exercises.First().ExerciseSets, Is.Not.Null);
            });
        }

        [Test]
        public async Task AddWorkoutAsync()
        {
            var dbContext = CreateDbContext();

            var workout = new Workout
            {
                Name = "name",
                Description = "description",
                UserId = "id",
            };

            var repository = new WorkoutRepository(dbContext);

            await repository.AddWorkoutAsync(workout);

            Assert.That(dbContext.Workout.Contains(workout));
        }

        [Test]
        public async Task UpdateWorkoutAsync()
        {
            var dbContext = CreateDbContext();

            var workout = new Workout
            {
                Name = "name",
                Description = "description",
                UserId = "id",
            };

            dbContext.AddContent(new List<Workout> { workout });

            var repository = new WorkoutRepository(dbContext);

            workout.Name = "new name";

            await repository.UpdateWorkoutAsync(workout);

            Assert.Multiple(() =>
            {
                Assert.That(!dbContext.Workout.All(x => x.Name == "name"));
                Assert.That(dbContext.Workout.All(x => x.Name == "new name"));
            });
        }

        [Test]
        public async Task DeleteWorkoutByIdAsync()
        {
            var workouts = new List<Workout>
            {
                new Workout
                {
                    Id = 1,
                    Description = "desc",
                    Name = "name",
                    UserId = "id",
                },
                new Workout
                {
                    Id = 2,
                    Description = "desc",
                    Name = "name",
                    UserId = "id",
                },
                new Workout
                {
                    Id = 3,
                    Description = "desc",
                    Name = "name",
                    UserId = "id2",
                }
            };
            var dbContext = CreateDbContext();

            await dbContext.AddContent(workouts);

            var repository = new WorkoutRepository(dbContext);

            const int Id = 2;

            await repository.DeleteWorkoutByIdAsync(Id);

            Assert.That(!dbContext.Workout.All(x => x.Id == Id));
        }
    }
}
