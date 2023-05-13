using Core.Entities;

namespace Core.Specification;

public class VoteWithSpecificationParams : BaseSpecification<Vote>
{
    public VoteWithSpecificationParams(VoteParams voteParams)
        : base(x => (!voteParams.UserId.HasValue || x.UserId == voteParams.UserId) &&
                    (!voteParams.VotingId.HasValue || x.VotingId == voteParams.VotingId) &&
                    (!voteParams.NameId.HasValue || x.NameId == voteParams.NameId)
        )
    {
        AddInclude(x => x.Voting);
        AddInclude(x => x.User);
        AddInclude(x => x.Name);
        ApplyPaging(voteParams.PageSize * (voteParams.PageNumber -1), voteParams.PageSize);
    }
}