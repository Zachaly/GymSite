using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using GymSite.Models.Response;
using GymSite.Models.User;
using GymSite.Models.User.Request;
using Microsoft.AspNetCore.Identity;

namespace GymSite.Application
{
    [Implementation(typeof(IUserService))]
    public class UserService : IUserService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserFactory _userFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly IUserRepository _userRepository;

        public UserService(IUserInfoRepository userInfoRepository,
            UserManager<ApplicationUser> userManager,
            IUserFactory userFactory,
            IResponseFactory responseFactory,
            IUserRepository userRepository)
        {
            _userInfoRepository = userInfoRepository;
            _userManager = userManager;
            _userFactory = userFactory;
            _responseFactory = responseFactory;
            _userRepository = userRepository;
        }

        public async Task<ResponseModel> AddUser(AddUserRequest request)
        {
            try
            {
                var user = _userFactory.Create(request);
                var userInfo = _userFactory.CreateInfo(request);

                await _userManager.CreateAsync(user, request.Password);

                userInfo.UserId = user.Id;

                await _userInfoRepository.AddInfoAsync(userInfo);

                user.UserInfoId = userInfo.Id;

                await _userManager.UpdateAsync(user);

                return _responseFactory.CreateSuccess("User created");
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFail(ex.Message, null);
            }
        }

        public async Task<DataResponseModel<UserModel>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id, user => _userFactory.CreateModel(user));

            return _responseFactory.CreateSuccess(user);
        }

        public async Task<ResponseModel> UpdateUser(UpdateUserRequest request, string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId, user => user);

            user.NickName = request.NickName ?? user.NickName;
            user.UserInfo.FirstName = request.FirstName ?? user.UserInfo.FirstName;
            user.UserInfo.LastName = request.LastName ?? user.UserInfo.LastName;
            user.UserInfo.Gender = request.Gender ?? user.UserInfo.Gender;

            await _userRepository.UpdateUser(user);

            return _responseFactory.CreateSuccess();
        }
    }
}
