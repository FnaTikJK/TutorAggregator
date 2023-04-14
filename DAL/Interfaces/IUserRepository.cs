using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetUserAsync(string login);
        public Task UpdateAndSaveAsync(User user);
    }
}
