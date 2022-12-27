using GymSite.Database.Repository;
using GymSite.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace GymSite.Tests.Unit.Repository
{
    [TestFixture]
    public class WorkoutExerciseRepositoryTests : DatabaseTest
    {
        [Test]
        public async Task AddWorkoutExerciseAsync()
        {
            var dbContext = CreateDbContext();

            var repository = new WorkoutExerciseRepository(dbContext);
            
            await dbContext.AddContent(new List<Exercise> { new Exercise { Id = 1, Name = "name", Description = "description" } });

            var exercise = new WorkoutExercise { ExerciseId = 1, WorkoutId = 2 };

            await repository.AddWorkoutExerciseAsync(exercise);

            Assert.That(dbContext.WorkoutExercise.Contains(exercise));
        }

        [Test]
        public async Task DeleteWorkoutExerciseByIdAsync()
        {
            var exercises = new List<WorkoutExercise>
            {
                new WorkoutExercise { Id = 1, ExerciseId = 2, WorkoutId = 3 },
                new WorkoutExercise { Id = 4, ExerciseId = 2, WorkoutId = 3 },
                new WorkoutExercise { Id = 5, ExerciseId = 2, WorkoutId = 3 },
            };
        

            var dbContext = CreateDbContext();
            await dbContext.AddContent(exercises);

            var repository = new WorkoutExerciseRepository(dbContext);

            const int Id = 4;

            await repository.DeleteWorkoutExerciseAsync(Id);

            Assert.That(!dbContext.WorkoutExercise.Any(x => x.Id == Id));
        }

        [Test]
        public async Task GetWorkoutExerciseById()
        {
            var exercise = new Exercise { Id = 2, Name = "name", Description = "description" };

            var exercises = new List<WorkoutExercise>
            {
                new WorkoutExercise { Id = 1, ExerciseId = 2, WorkoutId = 3, Exercise = exercise },
                new WorkoutExercise { Id = 4, ExerciseId = 2, WorkoutId = 3, Exercise = exercise },
                new WorkoutExercise { Id = 5, ExerciseId = 2, WorkoutId = 3, Exercise = exercise },
            };

            var dbContext = CreateDbContext();
            await dbContext.AddContent(exercises);

            var repository = new WorkoutExerciseRepository(dbContext);

            const int Id = 4;

            var res = repository.GetWorkoutExerciseById(Id, x => x);

            Assert.That(res, Is.EqualTo(exercises.First(x => x.Id == Id)));
        }
    }
}
