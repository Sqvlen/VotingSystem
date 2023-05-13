using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

[Table("Voting")]
public class Voting : BaseEntity
{
    public string Title { get; set; }
    public string Details { get; set; }
    public string Description { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;
    
    public int CountVote { get; set; }
    public List<Vote> Votes { get; set; } = new List<Vote>();
    public List<Name> Names { get; set; } = new List<Name>();

    public bool isClosed { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
}