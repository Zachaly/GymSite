using GymSite.Models.Workout.Requests;
using GymSite.Models.Workout.Validator;

namespace GymSite.Tests.Unit.Validator
{
    [TestFixture]
    public class AddWorkoutValidatorTests
    {
        [Test]
        public void ValidData_Pass()
        {
            var request = new AddWorkoutRequest
            {
                Description = "description",
                Name = "name",
            };

            var res = new AddWorkoutValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void OnlyRequiredFields_Pass()
        {
            var request = new AddWorkoutRequest
            {
                Name = "name",
            };

            var res = new AddWorkoutValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void EmptyData_Fail()
        {
            var request = new AddWorkoutRequest
            {
            };

            var res = new AddWorkoutValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void NameExceedsMaxLength_Fail()
        {
            var request = new AddWorkoutRequest
            {
                Description = "description",
                Name = new string('a', 31),
            };

            var res = new AddWorkoutValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void NameBelowMinLength_Fail()
        {
            var request = new AddWorkoutRequest
            {
                Description = "description",
                Name = new string('a', 2),
            };

            var res = new AddWorkoutValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void DescriptionExceedsMaxLength_Fail()
        {
            var request = new AddWorkoutRequest
            {
                Description = new string('a', 201),
                Name = "name",
            };

            var res = new AddWorkoutValidator().Validate(request);

            Assert.That(!res.IsValid);
        }
    }
}
