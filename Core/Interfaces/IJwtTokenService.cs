using Core.Entities;

namespace Core.Interfaces;

public interface IJwtTokenService
{
    string CreateToken(User user);
}