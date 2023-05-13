using Core.Entities;

namespace Core.Specification;

public class UserWithSpecificationParams : BaseSpecification<User>
{
    public UserWithSpecificationParams(UserParams userParams)
        : base(x => ((string.IsNullOrEmpty(userParams.Search) || x.Name.FirstName.ToLower().Contains(userParams.Search)) |
                    (string.IsNullOrEmpty(userParams.Search) || x.Name.LastName.ToLower().Contains(userParams.Search))) &&
                    (!userParams.UserId.HasValue || x.Id == userParams.UserId) &&
                    (string.IsNullOrEmpty(userParams.Username) || x.UserName == userParams.Username)
        )
    {
        AddInclude(x => x.Name);
        AddInclude(x => x.Votes);
        AddInclude(x => x.Votings);
        AddOrderBy(x => x.Id);
        ApplyPaging(userParams.PageSize * (userParams.PageNumber -1), userParams.PageSize);
    }
}