using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataBaseContext _dataBaseContext;
    private readonly IMapper _mapper;

    public UserRepository(DataBaseContext dataBaseContext, IMapper mapper)
    {
        _dataBaseContext = dataBaseContext;
        _mapper = mapper;
    }


    public async Task<User> GetUserByNameAsync(Name name)
    {
        return await _dataBaseContext.Users
            .Include(p => p.Name)
            .Include(p => p.Votes)
            .Include(p => p.Votings)
            .SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<bool> UserExists(string username)
    {
        return await _dataBaseContext.Users.AnyAsync(x => x.UserName == username.ToLower());
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _dataBaseContext.Users
            .Include(p => p.Name)
            .Include(p => p.Votes)
            .Include(p => p.Votings)
            .SingleOrDefaultAsync(x => x.UserName == username);
    }
}