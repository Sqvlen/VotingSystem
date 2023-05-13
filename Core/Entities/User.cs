namespace Core.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; }
    public Name Name { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    public List<Vote> Votes { get; set; } = new List<Vote>();
    public List<Voting> Votings { get; set; } = new List<Voting>();
}