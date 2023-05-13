using Core.Entities;

namespace Core.Specification;

public class NameWithSpecificationParams : BaseSpecification<Name>
{
    public NameWithSpecificationParams(NameParams nameParams) 
        : base(x => (string.IsNullOrEmpty(nameParams.Search) || x.FirstName.ToLower().Contains(nameParams.Search)) &&
                    (string.IsNullOrEmpty(nameParams.Search) || x.LastName.ToLower().Contains(nameParams.Search)) &&
                    (!nameParams.UserId.HasValue || x.UserId == nameParams.UserId)
        )
    {
        AddInclude(x => x.Votes);
        AddInclude(x => x.Votings);
        AddOrderBy(x => x.Id);
        ApplyPaging(nameParams.PageSize * (nameParams.PageNumber -1), nameParams.PageSize);
    }
}