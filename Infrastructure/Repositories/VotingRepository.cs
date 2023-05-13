using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VotingRepository : IVotingReporitory
{
    private readonly DataBaseContext _dataBaseContext;

    public VotingRepository(DataBaseContext dataBaseContext)
    {
        _dataBaseContext = dataBaseContext;
    }

    public async Task<Voting> GetVotingByIdWithIncludes(int votingId)
    {
        return await _dataBaseContext.Votings
            .Include(p => p.Names)
            .Include(p => p.Votes)
            .SingleOrDefaultAsync(x => x.Id == votingId);
    }
}