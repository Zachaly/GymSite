using GymSite.Domain.Entity;
using GymSite.Models.User;
using GymSite.Models.User.Request;

namespace GymSite.Application.User.Abstractions
{
    public interface IUserFactory
    {
        ApplicationUser Create(AddUserRequest request);
        UserInfo CreateInfo(AddUserRequest request);
        UserModel CreateModel(ApplicationUser user);
    }
}
