using Core.Entities;

namespace API.Dtos;

public class MemberDto : BaseDto
{
    public NameDto Name { get; set; }
    
    public List<VoteDto> Votes { get; set; }
    public List<VotingDto> Votings { get; set; }
}