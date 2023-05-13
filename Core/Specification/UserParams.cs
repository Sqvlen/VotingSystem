namespace Core.Specification;

public class UserParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 5;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string? Username { get; set; }
    public int? UserId { get; set; }
    
    public string? Sort { get; set; }
    private string _search;
    public string? Search
    { 
        get => _search;
        set => _search = value.ToLower();
    }
}