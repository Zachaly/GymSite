using GymSite.Models.Workout.Requests;
using GymSite.Models.Workout.Validator;

namespace GymSite.Tests.Unit.Validator
{
    [TestFixture]
    public class AddWorkoutExerciseValidatorTests
    {
        [Test]
        public void ValidData_Pass()
        {
            var request = new AddWorkoutExerciseRequest
            {
                WorkoutId = 1,
                ExerciseId = 2
            };

            var res = new AddWorkoutExerciseValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void EmptyData_Fail()
        {
            var request = new AddWorkoutExerciseRequest
            {
            };

            var res = new AddWorkoutExerciseValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void WorkoutIdBelow1_Fail()
        {
            var request = new AddWorkoutExerciseRequest
            {
                WorkoutId = 0,
                ExerciseId = 2
            };

            var res = new AddWorkoutExerciseValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void ExerciseIdBelow1_Fail()
        {
            var request = new AddWorkoutExerciseRequest
            {
                WorkoutId = 1,
                ExerciseId = 0
            };

            var res = new AddWorkoutExerciseValidator().Validate(request);

            Assert.That(!res.IsValid);
        }
    }
}
