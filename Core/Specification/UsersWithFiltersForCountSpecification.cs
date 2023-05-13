using Core.Entities;

namespace Core.Specification;

public class UsersWithFiltersForCountSpecification : BaseSpecification<User>
{
    public UsersWithFiltersForCountSpecification(UserParams userParams)
        : base(x => ((string.IsNullOrEmpty(userParams.Search) || x.Name.FirstName.ToLower().Contains(userParams.Search)) |
                     (string.IsNullOrEmpty(userParams.Search) || x.Name.LastName.ToLower().Contains(userParams.Search))) &&
                    (!userParams.UserId.HasValue || x.Id == userParams.UserId) &&
                    (string.IsNullOrEmpty(userParams.Username) || x.UserName == userParams.Username)
        )
    {
    }
}