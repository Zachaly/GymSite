using GymSite.Models.User.Request;
using GymSite.Models.User.Validator;

namespace GymSite.Tests.Unit.Validator
{
    [TestFixture]
    public class UpdateUserValidatorTests
    {
        [Test]
        public void ValidData_Pass()
        {
            var request = new UpdateUserRequest
            {
                FirstName = "fname",
                LastName = "lname",
                NickName = "nickname",
            };

            var res = new UpdateUserValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void OnlyRequiredData_Pass()
        {
            var request = new UpdateUserRequest
            {
                
            };

            var res = new UpdateUserValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void NicknameBelowMinLength_Fail()
        {
            var request = new UpdateUserRequest
            {
                NickName = new string('a', 4),
            };

            var res = new UpdateUserValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void FirstNameExceedsMaxLength_Fail()
        {
            var request = new UpdateUserRequest
            {
                FirstName = new string('a', 51),
            };

            var res = new UpdateUserValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void LastNameExceedsMaxLength_Fail()
        {
            var request = new UpdateUserRequest
            {
                LastName = new string('a', 51),
            };

            var res = new UpdateUserValidator().Validate(request);

            Assert.That(!res.IsValid);
        }
    }
}
