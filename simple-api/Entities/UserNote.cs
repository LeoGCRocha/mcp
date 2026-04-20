namespace SimpleApi.Entities;

public class UserNote
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}
