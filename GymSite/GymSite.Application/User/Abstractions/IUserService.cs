using GymSite.Models.Response;
using GymSite.Models.User.Request;

namespace GymSite.Application.User.Abstractions
{
    public interface IUserService
    {
        Task<ResponseModel> AddUser(AddUserRequest request);
    }
}
