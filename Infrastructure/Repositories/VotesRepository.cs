using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VotesRepository : IVotesRepository
{
    private readonly DataBaseContext _dataBaseContext;

    public VotesRepository(DataBaseContext dataBaseContext)
    {
        _dataBaseContext = dataBaseContext;
    }

    public async Task<Vote> GetUserVote(int sourceUserId, int targetVotingId, int nameid)
    {
        return await _dataBaseContext.Votes
            .Where(x => x.VotingId == targetVotingId)
            .Where(x => x.UserId == sourceUserId)
            .Where(x => x.NameId == nameid)
            .SingleOrDefaultAsync();
    }

    public async Task<Vote> GetVoteByUserIdAndVotingId(int sourceUserId, int targetVotingId)
    {
        return await _dataBaseContext.Votes
            .Where(x => x.VotingId == targetVotingId)
            .Where(x => x.UserId == sourceUserId)
            .SingleOrDefaultAsync();
    }
}