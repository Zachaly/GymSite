using GymSite.Domain.Entity;

namespace GymSite.Database.User
{
    public interface IUserRepository
    {
        Task<T?> GetUserByNameAsync<T>(string username, Func<ApplicationUser, T> selector);
    }
}
