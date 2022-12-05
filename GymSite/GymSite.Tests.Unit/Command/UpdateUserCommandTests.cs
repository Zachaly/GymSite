using GymSite.Application.Auth.Abstractions;
using GymSite.Application.User.Abstractions;
using GymSite.Application.User.Commands;
using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.User.Request;
using Moq;

namespace GymSite.Tests.Unit.Command
{
    [TestFixture]
    public class UpdateUserCommandTests
    {
        [Test]
        public async Task UpdateUser()
        {
            var user = new ApplicationUser { Id = "id", UserInfo = new UserInfo() };

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.GetCurrentUserId())
                .ReturnsAsync(user.Id);

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.UpdateUser(It.IsAny<UpdateUserRequest>(), It.IsAny<string>()))
                .Callback((UpdateUserRequest request, string id) =>
                {
                    user.UserInfo.FirstName = request.FirstName;
                    user.UserInfo.LastName = request.LastName;
                    user.NickName = request.NickName;
                    user.UserInfo.Gender = request.Gender ?? user.UserInfo.Gender;
                }).ReturnsAsync(new ResponseModel
                {
                    Code = ResponseCode.NoContent,
                    Errors = null,
                    Message = "msg",
                    Success = true,
                });

            var command = new UpdateUserCommand
            {
                NickName = "new nickname",
                FirstName = "new fname",
                Gender = Domain.Enum.Gender.Male,
                LastName = "new lname"
            };

            var res = await new UpdateUserHandler(userServiceMock.Object, authServiceMock.Object).Handle(command, new CancellationToken());

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(user.NickName, Is.EqualTo(command.NickName));
                Assert.That(user.UserInfo.FirstName, Is.EqualTo(command.FirstName));
            });
        }
    }
}
