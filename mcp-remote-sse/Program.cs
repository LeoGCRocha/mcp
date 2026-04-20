using mcp_shared;
using mcp_remote_sse;
using mcp_shared.Tools;
using ModelContextProtocol.Protocol;

//
// "C:/Users/leona/RiderProjects/mcp-servers": {
//     "allowedTools": [],
//     "mcpContextUris": [],
//     "mcpServers": {
//         "sseMcp": {
//             "command": "cmd",
//             "args": [
//             "/c",
//             "npx",
//             "mcp-remote",
//             "http://localhost:5218/",
//             "--header",
//             "X-API-KEY:sk-7fA9xQ2LmZp8Vw3KcR1dN6YtU4hGJ0bE"
//                 ]
//         }
//     }
var builder = WebApplication.CreateBuilder(args);

var sharedAssembly = typeof(UsersTools).Assembly;

var serverInfo = new Implementation
{
    Name = "DotnetMcpServerSSE", Version = "1.0.0",
};
builder.Services
    .AddMcpServer(mcp =>
    {
        mcp.ServerInfo = serverInfo;
    })
    .WithHttpTransport()
    .WithToolsFromAssembly(sharedAssembly)
    .WithResourcesFromAssembly(sharedAssembly)
    .WithPromptsFromAssembly(sharedAssembly);

builder.Services
    .AddHttpClient<UsersApiClient>(client =>
    {
        var baseAddr = builder.Configuration["API_BASE_ADDRESS"];
        
        if (string.IsNullOrWhiteSpace(baseAddr))
        {
            baseAddr = "http://localhost:5000";
        }

        client.BaseAddress = new Uri(baseAddr);
    });

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();
app.MapMcp();
app.Run();