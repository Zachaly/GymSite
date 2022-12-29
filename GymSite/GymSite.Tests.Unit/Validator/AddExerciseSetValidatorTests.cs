using GymSite.Models.Workout.Requests;
using GymSite.Models.Workout.Validator;

namespace GymSite.Tests.Unit.Validator
{
    [TestFixture]
    public class AddExerciseSetValidatorTests
    {
        [Test]
        public void ValidData_Pass()
        {
            var request = new AddExerciseSetRequest
            {
                Reps = 1,
                Weight = 2,
                WorkoutExerciseId = 3
            };

            var res = new AddExerciseSetValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void EmptyData_Fail()
        {
            var request = new AddExerciseSetRequest
            {
            };

            var res = new AddExerciseSetValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void ExerciseIdBelow1_Fail()
        {
            var request = new AddExerciseSetRequest
            {
                WorkoutExerciseId = 0,
                Reps = 1,
                Weight = 2
            };

            var res = new AddExerciseSetValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void RepsBelow1_Fail()
        {
            var request = new AddExerciseSetRequest
            {
                WorkoutExerciseId = 1,
                Reps = 0,
                Weight = 2
            };

            var res = new AddExerciseSetValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void WeightBelow1_Fail()
        {
            var request = new AddExerciseSetRequest
            {
                WorkoutExerciseId = 1,
                Reps = 2,
                Weight = 0
            };

            var res = new AddExerciseSetValidator().Validate(request);

            Assert.That(!res.IsValid);
        }
    }
}
