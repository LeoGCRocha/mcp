namespace mcp_shared.Dtos.Users;

public record UserNoteResponse(Guid Id, string Content, DateTime CreatedAtUtc);
