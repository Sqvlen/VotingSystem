using System.Runtime.Intrinsics.X86;
using Core.Entities;

namespace Core.Specification;

public class VotingWithSpecificationParams : BaseSpecification<Voting>
{
    public VotingWithSpecificationParams(VotingParams votingParams)
        : base(x => ((string.IsNullOrEmpty(votingParams.Search) || x.Title.ToLower().Contains(votingParams.Search)) ||
                    (string.IsNullOrEmpty(votingParams.Search) || x.Description.ToLower().Contains(votingParams.Search)) || 
                    (string.IsNullOrEmpty(votingParams.Search) || x.Details.ToLower().Contains(votingParams.Search))) &&
                    (!votingParams.UserId.HasValue || x.UserId == votingParams.UserId) &&
                    (!votingParams.VotingId.HasValue || x.Id == votingParams.VotingId)
        )
    {
        AddInclude(x => x.Votes);
        AddInclude(x => x.Names);
        AddOrderBy(x => x.isClosed);
        ApplyPaging((votingParams.PageNumber - 1) * votingParams.PageSize, votingParams.PageSize);

        if (!string.IsNullOrEmpty(votingParams.SortByVotes))
        {
            switch (votingParams.SortByVotes)
            {
                case "voteAsc":
                    AddOrderBy(p => p.CountVote);
                    break;
                case "voteDesc":
                    AddOrderByDescending(p => p.CountVote);
                    break;
                default:
                    AddOrderBy(x => x.isClosed);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(votingParams.SortByAlphabetical))
        {
            switch (votingParams.SortByAlphabetical)
            {
                case "alphaAsc":
                    AddOrderBy(p => p.Title);
                    break;
                case "alphaDesc":
                    AddOrderByDescending(p => p.Title);
                    break;
                default:
                    AddOrderBy(p => p.isClosed);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(votingParams.SortByDate))
        {
            switch (votingParams.SortByDate)
            {
                case "dateAsc":
                    AddOrderBy(p => p.Created);
                    break;
                case "dateDesc":
                    AddOrderByDescending(p => p.Created);
                    break;
                default:
                    AddOrderBy(p => p.isClosed);
                    break;
            }
        }
    }
}