using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

[Table("Vote")]
public class Vote : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }

    public Name Name { get; set; }
    public int NameId { get; set; }
    
    public int VotingId { get; set; }
    public Voting Voting { get; set; }
}