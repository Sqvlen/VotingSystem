using Core.Entities;

namespace Core.Interfaces;

public interface IVotesRepository
{
    Task<Vote> GetUserVote(int sourceUserId, int targetVotingId, int targetNameId);
    Task<Vote> GetVoteByUserIdAndVotingId(int sourceUserId, int targetVotingId);
}