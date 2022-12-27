using GymSite.Database.Repository;
using GymSite.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Tests.Unit.Repository
{
    [TestFixture]
    public class ExerciseSetRepositoryTests : DatabaseTest
    {
        [Test]
        public async Task AddExerciseSetAsync()
        {
            var dbContext = CreateDbContext();

            var repository = new ExerciseSetRepository(dbContext);

            var set = new ExerciseSet { ExerciseId = 2, Reps = 3, Weigth = 4 };

            await repository.AddExerciseSetAsync(set);

            Assert.That(dbContext.ExerciseSet.Contains(set));
        }

        [Test]
        public async Task RemoveExerciseSetAsync() 
        {
            var sets = new List<ExerciseSet>
            {
                new ExerciseSet { Id = 1, ExerciseId = 2, Reps = 3, Weigth = 4 },
                new ExerciseSet { Id = 2, ExerciseId = 2, Reps = 3, Weigth = 4 },
                new ExerciseSet { Id = 3, ExerciseId = 2, Reps = 3, Weigth = 4 },
            };

            var dbContext = CreateDbContext();
            await dbContext.AddContent(sets);

            var repository = new ExerciseSetRepository(dbContext);

            const int Id = 2;

            await repository.RemoveExerciseSetByIdAsync(Id);

            Assert.That(!dbContext.ExerciseSet.Any(x => x.Id == Id));
        }
    }
}
