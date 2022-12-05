using GymSite.Application.User.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using GymSite.Models.User;
using GymSite.Models.User.Request;

namespace GymSite.Application.User
{
    [Implementation(typeof(IUserFactory))]
    public class UserFactory : IUserFactory
    {
        public ApplicationUser Create(AddUserRequest request)
            => new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Username,
                Created = DateTime.Now,
                NickName = request.NickName,
            };

        public UserInfo CreateInfo(AddUserRequest request)
            => new UserInfo
            {
                FirstName = request.FirstName,
                Gender = request.Gender ?? Domain.Enum.Gender.NotSpecified,
                LastName = request.LastName
            };

        public UserModel CreateModel(ApplicationUser user)
            => new UserModel
            {
                Created = user.Created.ToString("dd.MM.yyyy"),
                FirstName = user.UserInfo?.FirstName ?? "",
                LastName = user.UserInfo?.LastName ?? "",
                NickName = user.NickName,
                Gender = user.UserInfo?.Gender ?? 0
            };
    }
}
