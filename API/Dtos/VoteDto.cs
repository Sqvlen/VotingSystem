using Core.Entities;

namespace API.Dtos;

public class VoteDto : BaseDto
{
    public int NameId { get; set; }
    public int VotingId { get; set; }
    public int UserId { get; set; }
}