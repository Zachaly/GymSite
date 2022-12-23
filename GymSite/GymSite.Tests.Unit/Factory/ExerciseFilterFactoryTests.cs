using GymSite.Application;
using GymSite.Domain.Entity;
using GymSite.Models.ExerciseFilter.Request;

namespace GymSite.Tests.Unit.Factory
{
    [TestFixture]
    public class ExerciseFilterFactoryTests
    {
        [Test]
        public void Create()
        {
            var factory = new ExerciseFilterFactory();

            var request = new AddFilterRequest { Name = "name" };

            var filter = factory.Create(request);

            Assert.That(filter.Name, Is.EqualTo(request.Name));
        }

        [Test]
        public void CreateModel()
        {
            var factory = new ExerciseFilterFactory();

            var filter = new ExerciseFilter { Name = "name" };

            var model = factory.CreateModel(filter);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(filter.Id));
                Assert.That(model.Name, Is.EqualTo(filter.Name));
            });
        }
    }
}
