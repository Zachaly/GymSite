using GymSite.Models.Record.Request;
using GymSite.Models.Record.Validator;

namespace GymSite.Tests.Unit.Validator
{
    [TestFixture]
    public class AddExerciseRecordValidatorTests
    {
        [Test]
        public void ValidData_Pass()
        {
            var request = new AddExerciseRecordRequest
            {
                Reps = 1,
                Weight = 2,
                ExerciseId = 3,
                UserId = "id"
            };

            var res = new AddExerciseRecordValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void RepsLowerThan1_Fail()
        {
            var request = new AddExerciseRecordRequest
            {
                Reps = 0,
                Weight = 2,
                ExerciseId = 3,
                UserId = "id"
            };

            var res = new AddExerciseRecordValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void WeightLowerThan1_Fail()
        {
            var request = new AddExerciseRecordRequest
            {
                Reps = 1,
                Weight = 0,
                ExerciseId = 3,
                UserId = "id"
            };

            var res = new AddExerciseRecordValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void ExerciseIdLowerThan1_Fail()
        {
            var request = new AddExerciseRecordRequest
            {
                Reps = 1,
                Weight = 2,
                ExerciseId = 0,
                UserId = "id"
            };

            var res = new AddExerciseRecordValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void UserIdEmpty_Fail()
        {
            var request = new AddExerciseRecordRequest
            {
                Reps = 1,
                Weight = 2,
                ExerciseId = 3,
                UserId = ""
            };

            var res = new AddExerciseRecordValidator().Validate(request);

            Assert.That(!res.IsValid);
        }
    }
}
