using GymSite.Application.Commands;
using GymSite.Domain.Entity;
using GymSite.Domain.Enum;
using GymSite.Models.Response;
using GymSite.Models.User;
using GymSite.Models.User.Request;
using System.Net;
using System.Net.Http.Json;

namespace GymSite.Tests.Integration
{
    public class UserControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetUser_By_Id_Success()
        {
            var registerRequest = new AddUserRequest
            {
                Email = "email@email.com",
                FirstName = "fname",
                NickName = "nick",
                LastName = "lname",
                Gender = Gender.Male,
                Username = "username",
                Password = "zaq1@WSX"
            };

            await _httpClient.PostAsJsonAsync("api/auth/register", registerRequest);

            var user = GetFromDatabase<ApplicationUser>().FirstOrDefault();
            user.UserInfo = GetFromDatabase<UserInfo>().FirstOrDefault();

            var response = await _httpClient.GetAsync("api/user/" + user.Id);

            var responseContent = await response.Content.ReadFromJsonAsync<DataResponseModel<UserModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(user.NickName, responseContent.Data.NickName);
            Assert.Equal(user.UserInfo.FirstName, responseContent.Data.FirstName);
            Assert.Equal(user.UserInfo.LastName, responseContent.Data.LastName);
            Assert.Equal(user.UserInfo.Gender, responseContent.Data.Gender);
            Assert.IsType<string>(responseContent.Data.Created);
        }

        [Fact]
        public async Task GetUser_By_Id_Fail()
        {
            var response = await _httpClient.GetAsync("api/user/id");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_Success()
        {
            await Authenticate();

            var request = new UpdateUserCommand
            {
                FirstName = "new fname",
                Gender = Gender.Male,
                LastName = "new lname",
                NickName = "new nick",
            };

            var response = await _httpClient.PutAsJsonAsync("api/user", request);

            var updatedUser = GetAuthenticatedUser();
            updatedUser.UserInfo = GetFromDatabase<UserInfo>().FirstOrDefault(x => x.Id == updatedUser.UserInfoId);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(updatedUser.NickName, request.NickName);
            Assert.Equal(updatedUser.UserInfo.FirstName, request.FirstName);
            Assert.Equal(updatedUser.UserInfo.LastName, request.LastName);
            Assert.Equal(updatedUser.UserInfo.Gender, request.Gender);
        }
    }
}
