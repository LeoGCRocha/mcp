namespace SimpleApi.Modules;

using SimpleApi.Dtos.Users;
using SimpleApi.Entities;
using SimpleApi.Infrastructure.Data;
using SimpleApi.Infrastructure.Mappings;

public class UsersModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (AppDbContext dbContext) =>
        {
            var users = await dbContext.Users
                .Include(user => user.Notes)
                .OrderBy(user => user.Name)
                .ToListAsync();

            return Results.Ok(users.Select(user => user.ToResponse()));
        })
            .WithName("GetUsers")
            .WithTags("Users");

        app.MapGet("/users/{id:guid}", async (Guid id, AppDbContext dbContext) =>
        {
            var user = await dbContext.Users
                .Include(existingUser => existingUser.Notes)
                .FirstOrDefaultAsync(existingUser => existingUser.Id == id);

            return user is null
                ? Results.NotFound(new { message = "User not found." })
                : Results.Ok(user.ToResponse());
        })
        .WithName("GetUserById")
        .WithTags("Users");

        app.MapPost("/users", async (CreateUserRequest request, AppDbContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
            {
                return Results.BadRequest(new { message = "Name and email are required." });
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = request.Email.Trim(),
                CreatedAtUtc = DateTime.UtcNow
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/users/{user.Id}", user.ToResponse());
        })
        .WithName("CreateUser")
        .WithTags("Users");

        app.MapPut("/users/{id:guid}", async (Guid id, UpdateUserRequest request, AppDbContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
            {
                return Results.BadRequest(new { message = "Name and email are required." });
            }

            var user = await dbContext.Users
                .Include(existingUser => existingUser.Notes)
                .FirstOrDefaultAsync(existingUser => existingUser.Id == id);

            if (user is null)
            {
                return Results.NotFound(new { message = "User not found." });
            }

            user.Name = request.Name.Trim();
            user.Email = request.Email.Trim();

            await dbContext.SaveChangesAsync();
            return Results.Ok(user.ToResponse());
        })
        .WithName("UpdateUser")
        .WithTags("Users");

        app.MapDelete("/users/{id:guid}", async (Guid id, AppDbContext dbContext) =>
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(existingUser => existingUser.Id == id);
            if (user is null)
            {
                return Results.NotFound(new { message = "User not found." });
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("DeleteUser")
        .WithTags("Users");

        app.MapPost("/users/{id:guid}/notes", async (Guid id, AddUserNoteRequest request, AppDbContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return Results.BadRequest(new { message = "Note content is required." });
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(existingUser => existingUser.Id == id);
            if (user is null)
            {
                return Results.NotFound(new { message = "User not found." });
            }

            var note = new UserNote
            {
                Id = Guid.NewGuid(),
                Content = request.Content.Trim(),
                CreatedAtUtc = DateTime.UtcNow,
                UserId = user.Id
            };

            dbContext.UserNotes.Add(note);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/users/{id}", new UserNoteResponse(note.Id, note.Content, note.CreatedAtUtc));
        })
        .WithName("AddUserNote")
        .WithTags("Users");
    }
}
