using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

[Table("Names")]
public class Name : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<Voting> Votings { get; set; }
    public List<Vote> Votes { get; set; }
    public int CountVotes { get; set; }

    public User User { get; set; }
    public int UserId { get; set; }
}