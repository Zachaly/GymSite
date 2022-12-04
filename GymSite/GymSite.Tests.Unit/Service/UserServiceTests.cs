using GymSite.Application.Response.Abstractions;
using GymSite.Application.User;
using GymSite.Application.User.Abstractions;
using GymSite.Database.User;
using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.User.Request;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Tests.Unit.Service
{
    [TestFixture]
    public class UserServiceTests
    {
        private static Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            return mgr;
        }

        [Test]
        public async Task AddUser_Success()
        {
            var userList = new List<ApplicationUser>();
            var userInfoList = new List<UserInfo>();

            var managerMock = MockUserManager();

            managerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback((ApplicationUser user, string password) =>
                    {
                        user.Id = Guid.NewGuid().ToString();
                        userList.Add(user);
                    });

            managerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()));

            var repositoryMock = new Mock<IUserInfoRepository>();

            repositoryMock.Setup(x => x.AddInfoAsync(It.IsAny<UserInfo>()))
                .Callback((UserInfo info) =>
                {
                    info.Id = 1;
                    userInfoList.Add(info);
                });

            var factoryMock = new Mock<IUserFactory>();
            factoryMock.Setup(x => x.Create(It.IsAny<AddUserRequest>())).Returns(new ApplicationUser());
            factoryMock.Setup(x => x.CreateInfo(It.IsAny<AddUserRequest>())).Returns(new UserInfo());

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<ResponseCode>(), It.IsAny<string>()))
                .Returns((ResponseCode code, string message) => new ResponseModel
                {
                    Success = true,
                    Code = code,
                    Message = message
                });

            var service = new UserService(repositoryMock.Object, managerMock.Object, factoryMock.Object, responseFactoryMock.Object);

            var request = new AddUserRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                Gender = Domain.Enum.Gender.Male,
                LastName = "lname",
                NickName = "nname",
                Password = "password",
                Username = "username",
            };

            var res = await service.AddUser(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(res.Code, Is.EqualTo(ResponseCode.NoContent));
                Assert.That(userList.Count, Is.EqualTo(1));
                Assert.That(userInfoList.Count, Is.EqualTo(1));
                Assert.That(userList.First().UserInfoId, Is.EqualTo(1));
            });
        }
    }
}
