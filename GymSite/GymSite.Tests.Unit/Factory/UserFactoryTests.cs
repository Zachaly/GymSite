using GymSite.Application.User;
using GymSite.Models.User.Request;

namespace GymSite.Tests.Unit.Factory
{
    [TestFixture]
    public class UserFactoryTests
    {
        [Test]
        public void Create()
        {
            var factory = new UserFactory();

            var request = new AddUserRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                Gender = Domain.Enum.Gender.Male,
                LastName = "lname",
                NickName = "nname",
                Username = "username",
            };

            var user = factory.Create(request);

            Assert.Multiple(() =>
            {
                Assert.That(user.UserName, Is.EqualTo(request.Username));
                Assert.That(user.Email, Is.EqualTo(request.Email));
            });
        }

        [Test]
        public void CreateInfo()
        {
            var factory = new UserFactory();

            var request = new AddUserRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                Gender = Domain.Enum.Gender.Male,
                LastName = "lname",
                NickName = "nname",
                Username = "username",
            };

            var info = factory.CreateInfo(request);

            Assert.Multiple(() =>
            {
                Assert.That(info.FirstName, Is.EqualTo(request.FirstName));
                Assert.That(info.LastName, Is.EqualTo(request.LastName));
                Assert.That(info.Gender, Is.EqualTo(request.Gender));
            });
        }
    }
}
