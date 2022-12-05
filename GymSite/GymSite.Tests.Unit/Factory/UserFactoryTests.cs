using GymSite.Application.User;
using GymSite.Domain.Entity;
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

        [Test]
        public void CreateModel()
        {
            var factory = new UserFactory();

            var user = new ApplicationUser
            {
                NickName = "nick",
                Created = DateTime.Now,
                UserInfo = new UserInfo
                {
                    FirstName = "fname",
                    Gender = Domain.Enum.Gender.Male,
                    Id = 1,
                    LastName = "lname",
                }
            };

            var model = factory.CreateModel(user);

            Assert.Multiple(() =>
            {
                Assert.That(model.NickName, Is.EqualTo(user.NickName));
                Assert.That(model.FirstName, Is.EqualTo(user.UserInfo.FirstName));
                Assert.That(model.LastName, Is.EqualTo(user.UserInfo.LastName));
                Assert.That(model.Created, Is.EqualTo(DateTime.Now.ToString("dd.MM.yyyy")));
                Assert.That(model.Gender, Is.EqualTo(user.UserInfo.Gender));
            });
        }
    }
}
