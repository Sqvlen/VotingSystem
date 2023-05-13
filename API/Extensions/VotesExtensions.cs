using Core.Entities;

namespace API.Extensions;

public static class VotesExtensions
{
    public static int GetCountVotes(this List<Vote> votes)
    {
        return votes.Count;
    }
}