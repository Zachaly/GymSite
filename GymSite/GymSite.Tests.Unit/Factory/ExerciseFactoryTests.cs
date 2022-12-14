using GymSite.Application;
using GymSite.Domain.Entity;
using GymSite.Models.Exercise.Request;

namespace GymSite.Tests.Unit.Factory
{
    [TestFixture]
    public class ExerciseFactoryTests
    {
        [Test]
        public void Create()
        {
            var factory = new ExerciseFactory();

            var request = new AddExerciseRequest
            {
                Name = "Test",
                UserId = "id",
                Description = "Description",
            };

            var exercise = factory.Create(request);

            Assert.Multiple(() =>
            {
                Assert.That(exercise.Name, Is.EqualTo(request.Name));
                Assert.That(exercise.UserId, Is.EqualTo(request.UserId));
                Assert.That(exercise.Description, Is.EqualTo(request.Description));
            });
        }

        [Test]
        public void Create_Model()
        {
            var factory = new ExerciseFactory();

            var exercise = new Exercise
            {
                Name = "name",
                Description = "desc",
                Id = 1,
                Records = new List<ExerciseRecord>
                {
                    new ExerciseRecord()
                },
            };

            var model = factory.CreateModel(exercise);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(exercise.Id));
                Assert.That(model.Name, Is.EqualTo(exercise.Name));
                Assert.That(model.Records, Is.Not.Null);
                Assert.That(model.Description, Is.EqualTo(exercise.Description));
            });
        }

        [Test]
        public void Create_ListItem()
        {
            var factory = new ExerciseFactory();

            var exercise = new Exercise
            {
                Name = "Test",
                Id = 1,
            };

            var model = factory.CreateListItem(exercise);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(exercise.Id));
                Assert.That(model.Name, Is.EqualTo(exercise.Name));
            });
        }
    }
}
