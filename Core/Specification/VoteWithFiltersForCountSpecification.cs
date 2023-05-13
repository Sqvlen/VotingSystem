using Core.Entities;

namespace Core.Specification;

public class VoteWithFiltersForCountSpecification : BaseSpecification<Vote>
{
    public VoteWithFiltersForCountSpecification(VoteParams voteParams)
        : base(x => (!voteParams.UserId.HasValue || x.UserId == voteParams.UserId) &&
                    (!voteParams.VotingId.HasValue || x.VotingId == voteParams.VotingId) &&
                    (!voteParams.NameId.HasValue || x.NameId == voteParams.NameId)
        )
    {
    }
}