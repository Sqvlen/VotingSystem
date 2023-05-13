using Core.Entities;

namespace API.Dtos;

public class UserDto : BaseDto
{
    public string Username { get; set; }
    public string Token { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}