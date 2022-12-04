using GymSite.Domain.Enum;

namespace GymSite.Domain.Entity
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender Gender { get; set; }
    }
}
