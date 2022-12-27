using GymSite.Application;
using GymSite.Domain.Entity;
using GymSite.Models.Workout.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Tests.Unit.Factory
{
    [TestFixture]
    public class ExerciseSetFactoryTests
    {
        [Test]
        public void Create()
        {
            var factory = new ExerciseSetFactory();

            var request = new AddExerciseSetRequest
            {
                Reps = 1,
                Weight = 2,
                WorkoutExerciseId = 3,
            };

            var set = factory.Create(request);

            Assert.Multiple(() =>
            {
                Assert.That(set.ExerciseId, Is.EqualTo(request.WorkoutExerciseId));
                Assert.That(set.Weigth, Is.EqualTo(request.Weight));
                Assert.That(set.Reps, Is.EqualTo(request.Reps));
            });
        }

        [Test]
        public void CreateModel()
        {
            var factory = new ExerciseSetFactory();

            var set = new ExerciseSet
            {
                Reps = 1,
                Id = 2,
                ExerciseId = 3,
                Weigth = 4,
            };

            var model = factory.CreateModel(set);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(set.Id));
                Assert.That(model.Reps, Is.EqualTo(set.Reps));
                Assert.That(model.Weight, Is.EqualTo(set.Weigth.ToString()));
            });
        }
    }
}
