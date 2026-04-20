namespace mcp_shared.Dtos.Users;

public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAtUtc,
    IReadOnlyCollection<UserNoteResponse> Notes);
