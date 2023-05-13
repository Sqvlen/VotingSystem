using Core.Entities;

namespace Core.Interfaces;

public interface IVotingReporitory
{
    Task<Voting> GetVotingByIdWithIncludes(int votingId);
}