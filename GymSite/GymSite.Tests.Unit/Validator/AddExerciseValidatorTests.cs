using GymSite.Models.Exercise.Request;
using GymSite.Models.Exercise.Validator;

namespace GymSite.Tests.Unit.Validator
{
    [TestFixture]
    public class AddExerciseValidatorTests
    {
        [Test]
        public void ValidData_Pass()
        {
            var request = new AddExerciseRequest
            {
                Name = "exercise",
                Description = "Description",
                FilterIds = new List<int> { 1, 2 },
                UserId = "userid"
            };

            var res = new AddExerciseValidator().Validate(request);
            
            Assert.That(res.IsValid);
        }

        [Test]
        public void EmptyData_Fail()
        {
            var request = new AddExerciseRequest();

            var res = new AddExerciseValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void NullableData_Pass()
        {
            var request = new AddExerciseRequest
            {
                Name = "exercise",
                UserId = "userid"
            };

            var res = new AddExerciseValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void NameExceedsMaxLength_Fail()
        {
            var request = new AddExerciseRequest
            {
                Name = new string('a', 101),
                Description = "Description",
                FilterIds = new List<int> { 1, 2 },
                UserId = "userid"
            };

            var res = new AddExerciseValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void NameUnderMinLength_Fail()
        {
            var request = new AddExerciseRequest
            {
                Name = new string('a', 4),
                Description = "Description",
                FilterIds = new List<int> { 1, 2 },
                UserId = "userid"
            };

            var res = new AddExerciseValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void DescriptionExceedsMaxLength_Fail()
        {
            var request = new AddExerciseRequest
            {
                Name = "exercise",
                Description = new string('a', 201),
                FilterIds = new List<int> { 1, 2 },
                UserId = "userid"
            };

            var res = new AddExerciseValidator().Validate(request);

            Assert.That(!res.IsValid);
        }
    }
}
