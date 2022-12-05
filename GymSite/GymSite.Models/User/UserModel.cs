using GymSite.Domain.Enum;

namespace GymSite.Models.User
{
    public class UserModel
    {
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Created { get; set; }
    }
}
