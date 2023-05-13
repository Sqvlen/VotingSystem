using Core.Entities;

namespace Core.Specification;

public class VotingsWithFiltersForCountSpecification : BaseSpecification<Voting>
{
    public VotingsWithFiltersForCountSpecification(VotingParams votingParams) 
        : base(x => ((string.IsNullOrEmpty(votingParams.Search) || x.Title.ToLower().Contains(votingParams.Search)) ||
                     (string.IsNullOrEmpty(votingParams.Search) || x.Description.ToLower().Contains(votingParams.Search)) || 
                     (string.IsNullOrEmpty(votingParams.Search) || x.Details.ToLower().Contains(votingParams.Search))) &&
                    (!votingParams.UserId.HasValue || x.UserId == votingParams.UserId) &&
                    (!votingParams.VotingId.HasValue || x.Id == votingParams.VotingId)
        )
    {
        
    }
}