namespace Core.Specification;

public class VotingParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 9;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public int? UserId { get; set; }
    public int? VotingId { get; set; }

    public string? SortByAlphabetical { get; set; }
    public string? SortByVotes { get; set; }
    public string? SortByDate { get; set; }
    private string _search;
    public string? Search
    { 
        get => _search;
        set => _search = value.ToLower();
    }
}