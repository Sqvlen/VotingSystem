using Core.Entities;

namespace API.Dtos;

public class VotingDto : BaseDto
{
    public string Title { get; set; }
    public string Details { get; set; }
    public string Description { get; set; }

    public DateTime Created { get; set; }
    
    public int CountVote { get; set; }
    public List<NameDto> Names { get; set; }
    public List<VoteDto> Votes { get; set; }

    public bool isClosed { get; set; }
}