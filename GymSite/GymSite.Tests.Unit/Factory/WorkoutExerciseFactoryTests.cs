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
    public class WorkoutExerciseFactoryTests
    {
        [Test]
        public void Create()
        {
            var factory = new WorkoutExerciseFactory();

            var request = new AddWorkoutExerciseRequest
            {
                ExerciseId = 1,
                WorkoutId = 2,
            };

            var exercise = factory.Create(request);

            Assert.Multiple(() =>
            {
                Assert.That(exercise.ExerciseId, Is.EqualTo(request.ExerciseId));
                Assert.That(exercise.WorkoutId, Is.EqualTo(request.WorkoutId));
            });
        }

        [Test]
        public void CreateModel_ExerciseSets_Valid()
        {
            var factory = new WorkoutExerciseFactory();

            var exercise = new WorkoutExercise
            {
                Id = 1,
                WorkoutId = 2,
                ExerciseId = 3,
                Exercise = new Exercise { Id = 1, Name = "name", Description = "desc" },
                ExerciseSets = new ExerciseSet[] {}
            };

            var model = factory.CreateModel(exercise);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(exercise.Id));
                Assert.That(model.ExerciseName, Is.EqualTo(exercise.Exercise.Name));
                Assert.That(model.ExerciseDescription, Is.EqualTo(exercise.Exercise.Description));
                Assert.That(model.Sets, Is.Not.Null);
            });
        }

        [Test]
        public void CreateModel_ExerciseSets_Null()
        {
            var factory = new WorkoutExerciseFactory();

            var exercise = new WorkoutExercise
            {
                Id = 1,
                WorkoutId = 2,
                ExerciseId = 3,
                Exercise = new Exercise { Id = 1, Name = "name", Description = "desc" },
                ExerciseSets = null
            };

            var model = factory.CreateModel(exercise);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(exercise.Id));
                Assert.That(model.ExerciseName, Is.EqualTo(exercise.Exercise.Name));
                Assert.That(model.ExerciseDescription, Is.EqualTo(exercise.Exercise.Description));
                Assert.That(model.Sets, Is.Not.Null);
            });
        }
    }
}
