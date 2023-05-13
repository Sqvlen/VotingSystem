using Core.Entities;

namespace Core.Specification;

public class NameWithFiltersForCountSpecification : BaseSpecification<Name>
{
    public NameWithFiltersForCountSpecification(NameParams nameParams) 
        : base(x => (string.IsNullOrEmpty(nameParams.Search) || x.FirstName.ToLower().Contains(nameParams.Search)) &&
                    (string.IsNullOrEmpty(nameParams.Search) || x.LastName.ToLower().Contains(nameParams.Search)) &&
                    (!nameParams.UserId.HasValue || x.UserId == nameParams.UserId)
    )
    {
        
    }
}