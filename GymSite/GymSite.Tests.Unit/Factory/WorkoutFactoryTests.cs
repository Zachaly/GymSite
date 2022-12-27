using GymSite.Application;
using GymSite.Domain.Entity;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;

namespace GymSite.Tests.Unit.Factory
{
    [TestFixture]
    public class WorkoutFactoryTests
    {
        [Test]
        public void Create()
        {
            var factory = new WorkoutFactory();
            var request = new AddWorkoutRequest
            {
                UserId = "id",
                Description = "description",
                Name = "name",
            };

            var workout = factory.Create(request);

            Assert.Multiple(() =>
            {
                Assert.That(workout.UserId, Is.EqualTo(request.UserId));
                Assert.That(workout.Name, Is.EqualTo(request.Name));
                Assert.That(workout.Description, Is.EqualTo(request.Description));
            });
        }

        [Test]
        public void CreateModel()
        {
            var factory = new WorkoutFactory();
            var workout = new Workout
            {
                Description = "description",
                Id = 1,
                Name = "name",
                UserId = "id",
                Exercises = new List<WorkoutExercise>
                {
                    new WorkoutExercise
                    {
                        Id = 2,
                        Exercise = new Exercise { Name = "ex_name", Id = 3, Description = "ex_desc" },
                        ExerciseSets= new List<ExerciseSet> 
                        {
                            new ExerciseSet { Id = 4, ExerciseId = 3, Reps = 5, Weigth = 6 }
                        }
                    }
                }
            };

            var model = factory.CreateModel(workout);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(workout.Id));
                Assert.That(model.Description, Is.EqualTo(workout.Description));
                Assert.That(model.Name, Is.EqualTo(workout.Name));
                Assert.That(model.Exercices, Is.TypeOf<List<WorkoutExerciseModel>>());
            });
        }

        [Test]
        public void CreateListItem()
        {
            var factory = new WorkoutFactory();
            var workout = new Workout
            {
                Id = 1,
                Name = "name",
            };

            var item = factory.CreateListItem(workout);

            Assert.Multiple(() =>
            {
                Assert.That(item.Id, Is.EqualTo(workout.Id));
                Assert.That(item.Name, Is.EqualTo(workout.Name));
            });
        }
    }
}
