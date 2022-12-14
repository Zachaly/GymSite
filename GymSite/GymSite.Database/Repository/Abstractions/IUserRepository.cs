using GymSite.Domain.Entity;

namespace GymSite.Database.Repository.Abstractions
{
    public interface IUserRepository
    {
        Task<T> GetUserByNameAsync<T>(string username, Func<ApplicationUser, T> selector);
        Task<T> GetUserByIdAsync<T>(string id, Func<ApplicationUser, T> selector);
        Task UpdateUser(ApplicationUser user);
    }
}
