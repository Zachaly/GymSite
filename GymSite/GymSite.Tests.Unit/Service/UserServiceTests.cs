using GymSite.Application.Response.Abstractions;
using GymSite.Application.User;
using GymSite.Application.User.Abstractions;
using GymSite.Database.User;
using GymSite.Domain.Entity;
using GymSite.Domain.Enum;
using GymSite.Models.Response;
using GymSite.Models.User;
using GymSite.Models.User.Request;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework.Constraints;

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

            var userRepositoryMock = new Mock<IUserRepository>();

            var service = new UserService(repositoryMock.Object, managerMock.Object, factoryMock.Object, responseFactoryMock.Object, userRepositoryMock.Object);

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

        [Test]
        public async Task GetUserById()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser {Id = "id1", UserName = "name1", NickName = "nick1" },
                new ApplicationUser {Id = "id2", UserName = "name2", NickName = "nick2" },
                new ApplicationUser {Id = "id3", UserName = "name3", NickName = "nick3" },
            };

            var managerMock = MockUserManager();

            var repositoryMock = new Mock<IUserInfoRepository>();

            var factoryMock = new Mock<IUserFactory>();
            factoryMock.Setup(x => x.CreateModel(It.IsAny<ApplicationUser>()))
                .Returns((ApplicationUser user) => new UserModel 
                    { 
                        NickName = user.NickName,
                        Created = "",
                        FirstName = "",
                        LastName = "",
                        Gender = 0
                    });

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<ResponseCode>(), It.IsAny<string>(), It.IsAny<UserModel>()))
                .Returns((ResponseCode code, string message, UserModel data) => new DataResponseModel<UserModel>
                {
                    Success = true,
                    Code = code,
                    Message = message,
                    Data = data
                });

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>(), It.IsAny<Func<ApplicationUser, UserModel>>()))
                .ReturnsAsync((string id, Func<ApplicationUser, UserModel> selector)
                    => new UserModel { NickName = userList.FirstOrDefault(x => x.Id == id).NickName });

            var service = new UserService(repositoryMock.Object, managerMock.Object, factoryMock.Object, responseFactoryMock.Object, userRepositoryMock.Object);

            var t = factoryMock.Object.CreateModel(userList[1]);

            const string Id = "id2";

            var res = await service.GetUserById(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(res.Code, Is.EqualTo(ResponseCode.Ok));
                Assert.That(res.Data.NickName, Is.EqualTo(userList.FirstOrDefault(x => x.Id == Id).NickName));
            });
        }

        [Test]
        public async Task UpdateUser_FullContentRequest()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser {Id = "id1", UserName = "name1", NickName = "nick1", UserInfo = new UserInfo() },
                new ApplicationUser {Id = "id2", UserName = "name2", NickName = "nick2", UserInfo = new UserInfo() },
                new ApplicationUser {Id = "id3", UserName = "name3", NickName = "nick3", UserInfo = new UserInfo() },
            };

            var managerMock = MockUserManager();

            var repositoryMock = new Mock<IUserInfoRepository>();

            var factoryMock = new Mock<IUserFactory>();

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<ResponseCode>(), It.IsAny<string>()))
                .Returns((ResponseCode code, string message) => new ResponseModel
                {
                    Success = true,
                    Code = code,
                    Message = message,
                });

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<ApplicationUser>()));
            userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>(), It.IsAny<Func<ApplicationUser, ApplicationUser>>()))
                .Returns((string id, Func<ApplicationUser, ApplicationUser> selector)
                    => Task.FromResult(userList.FirstOrDefault(x => x.Id == id)));


            var service = new UserService(repositoryMock.Object, managerMock.Object, factoryMock.Object, responseFactoryMock.Object, userRepositoryMock.Object);

            const string Id = "id2";
            var request = new UpdateUserRequest
            {
                FirstName = "new fname",
                Gender = Domain.Enum.Gender.Male,
                LastName = "new lname",
                NickName = "new nname"
            };

            var res = await service.UpdateUser(request, Id);

            var user = userList.FirstOrDefault(x => x.Id == Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(res.Code, Is.EqualTo(ResponseCode.NoContent));
                Assert.That(user.NickName, Is.EqualTo(request.NickName));
                Assert.That(user.UserInfo.Gender, Is.EqualTo(request.Gender));
                Assert.That(user.UserInfo.FirstName, Is.EqualTo(request.FirstName));
                Assert.That(user.UserInfo.LastName, Is.EqualTo(request.LastName));
            });
        }

        [Test]
        public async Task UpdateUser_AllNullContentRequest()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser {Id = "id1", UserName = "name1", NickName = "nick1", UserInfo = new UserInfo() },
                new ApplicationUser {
                    Id = "id2",
                    UserName = "name2",
                    NickName = "nick2",
                    UserInfo = new UserInfo
                    {
                        FirstName = "fname",
                        LastName = "lname",
                        Gender = Gender.Male,
                    }},
                new ApplicationUser {Id = "id3", UserName = "name3", NickName = "nick3", UserInfo = new UserInfo() },
            };

            var managerMock = MockUserManager();

            var repositoryMock = new Mock<IUserInfoRepository>();

            var factoryMock = new Mock<IUserFactory>();

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<ResponseCode>(), It.IsAny<string>()))
                .Returns((ResponseCode code, string message) => new ResponseModel
                {
                    Success = true,
                    Code = code,
                    Message = message,
                });

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<ApplicationUser>()));
            userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>(), It.IsAny<Func<ApplicationUser, ApplicationUser>>()))
                .Returns((string id, Func<ApplicationUser, ApplicationUser> selector)
                    => Task.FromResult(userList.FirstOrDefault(x => x.Id == id)));


            var service = new UserService(repositoryMock.Object, managerMock.Object, factoryMock.Object, responseFactoryMock.Object, userRepositoryMock.Object);

            const string Id = "id2";
            var request = new UpdateUserRequest();

            var user = userList.FirstOrDefault(x => x.Id == Id);

            var OldNick = user.NickName;
            var OldFName = user.UserInfo.FirstName;
            var OldLName = user.UserInfo.LastName;
            var OldGender = user.UserInfo.Gender;

            var res = await service.UpdateUser(request, Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(res.Code, Is.EqualTo(ResponseCode.NoContent));
                Assert.That(user.NickName, Is.EqualTo(OldNick));
                Assert.That(user.UserInfo.Gender, Is.EqualTo(OldGender));
                Assert.That(user.UserInfo.FirstName, Is.EqualTo(OldFName));
                Assert.That(user.UserInfo.LastName, Is.EqualTo(OldLName));
            });
        }
    }
}
