namespace Core.Specification;

public class VoteParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 5;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    
    public int? UserId { get; set; }
    public int? VotingId { get; set; }
    public int? VoteId { get; set; }
    public int? NameId { get; set; }
}