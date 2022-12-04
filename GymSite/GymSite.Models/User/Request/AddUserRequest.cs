using GymSite.Domain.Enum;

namespace GymSite.Models.User.Request
{
    public class AddUserRequest
    {
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public string Username { get; set; }
    }
}
