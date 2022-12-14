using GymSite.Application;
using GymSite.Application.Abstractions;
using GymSite.Application.Commands;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.User.Response;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GymSite.Tests.Unit.Command
{
    [TestFixture]
    public class LoginCommandTests
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
        public async Task LoginCommand_Success()
        {
            var user = new ApplicationUser { Id = "id", UserName = "username", Email = "email@email.com" };
            const string Password = "password";

            var userManagerMock = MockUserManager();

            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser givenUser, string password)
                    => user.UserName == givenUser.UserName && Password == password);

            userManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<Claim> { new Claim("Role", "Admin") });

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<LoginResponse>(), It.IsAny<string>()))
                .Returns((LoginResponse data, string message) 
                    => new DataResponseModel<LoginResponse>
                    {
                        Data = data,
                        ValidationErrors = null,
                        Message = message,
                        Success = true
                    }
                );

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByNameAsync(It.IsAny<string>(), It.IsAny<Func<ApplicationUser, ApplicationUser>>()))
                .ReturnsAsync(user);

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.CreateToken(It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(new JwtSecurityToken());

            var command = new LoginCommand
            {
                Password = "password",
                Username = "username",
            };

            var handler = new LoginHandler(userManagerMock.Object, responseFactoryMock.Object,
                authServiceMock.Object, userRepositoryMock.Object);

            var res = await handler.Handle(command, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(res.Data.AuthToken, Is.Not.Empty);
                Assert.That(res.Data.UserId, Is.Not.Empty);
                Assert.That(res.Data.Username, Is.EqualTo(command.Username));
                Assert.That(res.Data.Claims.Contains("Admin"));
            });
        }

        [Test]
        public async Task LoginCommand_Fail_PasswordIncorrect()
        {
            var user = new ApplicationUser { Id = "id", UserName = "username", Email = "email@email.com" };
            const string Password = "password";

            var userManagerMock = MockUserManager();

            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser givenUser, string password)
                    => user.UserName == givenUser.UserName && Password == password);

            userManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<Claim>());

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFail<LoginResponse>(It.IsAny<string>(), null))
                .Returns((string message, object _)
                    => new DataResponseModel<LoginResponse>
                    {
                        Data = null,
                        ValidationErrors = null,
                        Message = message,
                        Success = false
                    }
                );

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByNameAsync(It.IsAny<string>(), It.IsAny<Func<ApplicationUser, ApplicationUser>>()))
                .ReturnsAsync(user);

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.CreateToken(It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(new JwtSecurityToken());

            var command = new LoginCommand
            {
                Password = "passw",
                Username = "username",
            };

            var handler = new LoginHandler(userManagerMock.Object, responseFactoryMock.Object,
                authServiceMock.Object, userRepositoryMock.Object);

            var res = await handler.Handle(command, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.That(!res.Success);
                Assert.That(res.Data, Is.Null);
            });
        }

        [Test]
        public async Task LoginCommand_Fail_UserNameIncorrect()
        {
            ApplicationUser user = null;
            const string Password = "password";

            var userManagerMock = MockUserManager();

            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser givenUser, string password)
                    => user.UserName == givenUser.UserName && Password == password);

            userManagerMock.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<Claim>());
                
            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFail<LoginResponse>(It.IsAny<string>(), null))
                .Returns((string message, object _)
                    => new DataResponseModel<LoginResponse>
                    {
                        Data = null,
                        ValidationErrors = null,
                        Message = message,
                        Success = false
                    }
                );

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByNameAsync(It.IsAny<string>(), It.IsAny<Func<ApplicationUser, ApplicationUser>>()))
                .ReturnsAsync(user);

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.CreateToken(It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(new JwtSecurityToken());

            var command = new LoginCommand
            {
                Password = "password",
                Username = "user",
            };

            var handler = new LoginHandler(userManagerMock.Object, responseFactoryMock.Object,
                authServiceMock.Object, userRepositoryMock.Object);

            var res = await handler.Handle(command, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.That(!res.Success);
                Assert.That(res.Data, Is.Null);
            });
        }
    }
}
