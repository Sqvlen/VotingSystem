using Core.Entities;

namespace Core.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> GetUserByNameAsync(Name name);
    Task<bool> UserExists(string username);
}