namespace SimpleApi.Dtos.Users;

public record UserNoteResponse(Guid Id, string Content, DateTime CreatedAtUtc);
