using GymSite.Models.User.Request;
using GymSite.Models.User.Validator;

namespace GymSite.Tests.Unit.Validator
{
    [TestFixture]
    public class AddUserValidatorTests
    {
        [Test]
        public void ValidData_Pass()
        {
            var request = new AddUserRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                LastName = "lname",
                NickName = "nickname",
                Password = "zaq1@WSX",
                Username = "username"
            };

            var res = new AddUserValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void OnlyRequiredData_Pass()
        {
            var request = new AddUserRequest
            {
                Email = "email@email.com",
                NickName = "nickname",
                Password = "zaq1@WSX",
                Username = "username"
            };

            var res = new AddUserValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void EmptyData_Fail()
        {
            var request = new AddUserRequest
            {

            };

            var res = new AddUserValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void InvalidEmail_Fail()
        {
            var request = new AddUserRequest
            {
                Email = "email",
                NickName = "nickname",
                Password = "zaq1@WSX",
                Username = "username"
            };

            var res = new AddUserValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void NicknameBelowMinLength_Fail()
        {
            var request = new AddUserRequest
            {
                Email = "email@email.com",
                NickName = new string('a', 4),
                Password = "zaq1@WSX",
                Username = "username"
            };

            var res = new AddUserValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void PasswordBelowMinLength_Fail()
        {
            var request = new AddUserRequest
            {
                Email = "email@email.com",
                NickName = "nickname",
                Password = new string('a', 5),
                Username = "username"
            };

            var res = new AddUserValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void FirstNameExceedsMaxLength_Fail()
        {
            var request = new AddUserRequest
            {
                Email = "email@email.com",
                NickName = "nickname",
                Password = "zaq1@WSX",
                Username = "username",
                FirstName = new string('a', 51),
            };

            var res = new AddUserValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void LastNameExceedsMaxLength_Fail()
        {
            var request = new AddUserRequest
            {
                Email = "email@email.com",
                NickName = "nickname",
                Password = "zaq1@WSX",
                Username = "username",
                LastName = new string('a', 51),
            };

            var res = new AddUserValidator().Validate(request);

            Assert.That(!res.IsValid);
        }
    }
}
