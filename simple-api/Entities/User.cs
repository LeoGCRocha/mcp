namespace SimpleApi.Entities;

public class User
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public List<UserNote> Notes { get; set; } = [];
}
