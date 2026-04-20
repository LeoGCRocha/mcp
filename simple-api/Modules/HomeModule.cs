namespace SimpleApi.Modules;

public class HomeModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => Results.Ok(new
        {
            message = "Simple API is running",
            swagger = "/swagger",
            endpoints = new[]
            {
                "GET /users",
                "GET /users/{id}",
                "POST /users",
                "PUT /users/{id}",
                "DELETE /users/{id}",
                "POST /users/{id}/notes"
            }
        }))
        .WithTags("Home");
    }
}
