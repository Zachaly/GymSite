using GymSite.Domain.Enum;

namespace GymSite.Models.User.Request
{
    public class UpdateUserRequest
    {
        public string? NickName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
    }
}
