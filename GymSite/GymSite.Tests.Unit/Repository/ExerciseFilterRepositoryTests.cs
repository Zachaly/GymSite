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
    public class ExerciseFilterRepositoryTests : DatabaseTest
    {
        [Test]
        public async Task GetFilters()
        {
            var filters = new List<ExerciseFilter>
            {
                new ExerciseFilter { Name = "name1" },
                new ExerciseFilter { Name = "name2" },
                new ExerciseFilter { Name = "name3" },
            };

            var dbContext = CreateDbContext();

            await dbContext.AddContent(filters);

            var repository = new ExerciseFilterRepository(dbContext);

            var res = repository.GetFilters(x => x);

            Assert.That(res, Is.EquivalentTo(filters));
        }

        [Test]
        public async Task AddFilter()
        {
            var dbContext = CreateDbContext();

            var filter = new ExerciseFilter { Name = "name" };

            var repository = new ExerciseFilterRepository(dbContext);

            await repository.AddFilter(filter);

            Assert.That(dbContext.ExerciseFilter.Contains(filter));
        }

        [Test]
        public async Task RemoveFilter()
        {
            var filters = new List<ExerciseFilter>
            {
                new ExerciseFilter { Name = "name1", Id = 1 },
                new ExerciseFilter { Name = "name2", Id = 2 },
                new ExerciseFilter { Name = "name3", Id = 3 },
            };

            var dbContext = CreateDbContext();

            await dbContext.AddContent(filters);

            var repository = new ExerciseFilterRepository(dbContext);

            const int Id = 2;

            await repository.RemoveFilter(Id);

            Assert.That(!dbContext.ExerciseFilter.Any(x => x.Id == Id));
        }
    }
}
