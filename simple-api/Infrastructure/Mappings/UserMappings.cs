namespace SimpleApi.Infrastructure.Mappings;

using SimpleApi.Dtos.Users;
using SimpleApi.Entities;

public static class UserMappings
{
    public static UserResponse ToResponse(this User user) =>
        new(
            user.Id,
            user.Name,
            user.Email,
            user.CreatedAtUtc,
            user.Notes
                .OrderByDescending(note => note.CreatedAtUtc)
                .Select(note => new UserNoteResponse(note.Id, note.Content, note.CreatedAtUtc))
                .ToArray());
}
