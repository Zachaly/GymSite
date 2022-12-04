using Microsoft.AspNetCore.Identity;

namespace GymSite.Domain.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }
        public int? UserInfoId { get; set; }
        public UserInfo? UserInfo { get; set; }
        public DateTime Created { get; set; }
    }
}
