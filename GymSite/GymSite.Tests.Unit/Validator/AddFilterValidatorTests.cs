using GymSite.Models.ExerciseFilter.Request;
using GymSite.Models.ExerciseFilter.Validator;

namespace GymSite.Tests.Unit.Validator
{
    [TestFixture]
    public class AddFilterValidatorTests
    {
        [Test]
        public void ValidData_Pass()
        {
            var request = new AddFilterRequest
            {
                Name = "filter"
            };

            var res = new AddFilterValidator().Validate(request);

            Assert.That(res.IsValid);
        }
        [Test]
        public void EmptyName_Fail()
        {
            var request = new AddFilterRequest
            {
                
            };

            var res = new AddFilterValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void NameExceedsMaxLength_Fail()
        {
            var request = new AddFilterRequest
            {
                Name = new string('a', 21)
            };

            var res = new AddFilterValidator().Validate(request);

            Assert.That(!res.IsValid);
        }
    }
}
