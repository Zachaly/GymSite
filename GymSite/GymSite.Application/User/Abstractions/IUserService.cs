using GymSite.Models.Response;
using GymSite.Models.User;
using GymSite.Models.User.Request;

namespace GymSite.Application.Abstractions
{
    public interface IUserService
    {
        Task<ResponseModel> AddUser(AddUserRequest request);
        Task<DataResponseModel<UserModel>> GetUserById(string id);
        Task<ResponseModel> UpdateUser(UpdateUserRequest request, string userId);
    }
}
