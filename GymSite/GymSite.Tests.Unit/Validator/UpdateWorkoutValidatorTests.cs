using GymSite.Models.Workout.Requests;
using GymSite.Models.Workout.Validator;

namespace GymSite.Tests.Unit.Validator
{
    public class UpdateWorkoutValidatorTests
    {
        [Test]
        public void ValidData_Pass()
        {
            var request = new UpdateWorkoutRequest
            {
                Id = 1,
                Description = "description",
                Name = "name",
            };

            var res = new UpdateWorkoutValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void OnlyRequiredFields_Pass()
        {
            var request = new UpdateWorkoutRequest
            {
                Id = 1,
                Name = "name",
            };

            var res = new UpdateWorkoutValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void EmptyData_Fail()
        {
            var request = new UpdateWorkoutRequest
            {
            };

            var res = new UpdateWorkoutValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void NameExceedsMaxLength_Fail()
        {
            var request = new UpdateWorkoutRequest
            {
                Id = 1,
                Description = "description",
                Name = new string('a', 31),
            };

            var res = new UpdateWorkoutValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void NameBelowMinLength_Fail()
        {
            var request = new UpdateWorkoutRequest
            {
                Id = 1,
                Description = "description",
                Name = new string('a', 2),
            };

            var res = new UpdateWorkoutValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void DescriptionExceedsMaxLength_Fail()
        {
            var request = new UpdateWorkoutRequest
            {
                Id = 1,
                Description = new string('a', 201),
                Name = "name",
            };

            var res = new UpdateWorkoutValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void IdBelow1_Fail()
        {
            var request = new UpdateWorkoutRequest
            {
                Id = 0,
                Description = "description",
                Name = "name"
            };

            var res = new UpdateWorkoutValidator().Validate(request);

            Assert.That(!res.IsValid);
        }
    }
}
