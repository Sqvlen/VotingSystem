using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NameRepository : INameRepository
{
    private readonly DataBaseContext _dataBaseContext;

    public NameRepository(DataBaseContext dataBaseContext)
    {
        _dataBaseContext = dataBaseContext;
    }

    public async Task<Name> GetNameByIdWithIncludes(int nameId)
    {
        return await _dataBaseContext.Names
            .Include(p => p.Votes)
            .Include(p => p.Votings)
            .SingleOrDefaultAsync(x => x.Id == nameId);
    }
}