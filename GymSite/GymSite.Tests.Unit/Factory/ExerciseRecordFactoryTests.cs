using GymSite.Application;
using GymSite.Domain.Entity;
using GymSite.Models.Record.Request;

namespace GymSite.Tests.Unit.Factory
{
    [TestFixture]
    public class ExerciseRecordFactoryTests
    {
        [Test]
        public void Create()
        {
            var factory = new ExerciseRecordFactory();

            var request = new AddExerciseRecordRequest
            {
                ExerciseId = 1,
                Reps = 2,
                UserId = "id",
                Weight = 3
            };

            var record = factory.Create(request);

            Assert.Multiple(() =>
            {
                Assert.That(record.ExerciseId, Is.EqualTo(request.ExerciseId));
                Assert.That(record.Reps, Is.EqualTo(request.Reps));
                Assert.That(record.UserId, Is.EqualTo(request.UserId));
                Assert.That(record.Weight, Is.EqualTo(request.Weight));
            });
        }

        [Test]
        public void CreateModel()
        {
            var factory = new ExerciseRecordFactory();

            var record = new ExerciseRecord
            {
                Reps = 2,
                Weight = 3,
                Id = 4
            };

            var model = factory.CreateModel(record);

            Assert.Multiple(() =>
            {
                Assert.That(model.Reps, Is.EqualTo(record.Reps));
                Assert.That(model.Weight, Is.EqualTo(record.Weight.ToString()));
                Assert.That(model.Id, Is.EqualTo(record.Id));
            });
        }
    }
}
