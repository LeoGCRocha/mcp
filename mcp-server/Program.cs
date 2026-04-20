using mcp_shared;
using mcp_shared.Prompt;
using mcp_shared.Resources;
using mcp_shared.Tools;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
LoadDotEnv();

// "C:/Users/leona/RiderProjects/mcp-servers": {
//     "allowedTools": [],
//     "mcpContextUris": [],
//     "mcpServers": {
//         "usersApi": {
//             "type": "stdio",
//             "command": "C:\\Users\\leona\\RiderProjects\\mcp-servers\\mcp-server\\bin\\Debug\\net10.0\\mcp-server.exe",
//             "args": [],
//             "env": {
//                 "API_BASE_ADDRESS": "http://localhost:5000"
//             }
//         }
//     },

builder.Logging.ClearProviders();
builder.Configuration.AddEnvironmentVariables();

var serverInfo = new Implementation
{
    Name = "Dotnet Default MCP Server",
    Version = "1.0.0"
};

builder.Services
    .AddMcpServer(mcp =>
    {
        mcp.ServerInfo = serverInfo;
    })
    .WithStdioServerTransport()
    .WithResourcesFromAssembly(typeof(ApiDocumentationResource).Assembly)
    .WithPromptsFromAssembly(typeof(UsersPrompts).Assembly)
    .WithToolsFromAssembly(typeof(UsersTools).Assembly);
    
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

app.Run();

static void LoadDotEnv()
{
    foreach (var envFilePath in GetDotEnvCandidates())
    {
        if (!File.Exists(envFilePath))
        {
            continue;
        }

        foreach (var rawLine in File.ReadAllLines(envFilePath))
        {
            var line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
            {
                continue;
            }

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim().Trim('"');

            if (!string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(key)))
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }

        return;
    }
}

static IEnumerable<string> GetDotEnvCandidates()
{
    yield return Path.Combine(Directory.GetCurrentDirectory(), ".env");
    yield return Path.Combine(AppContext.BaseDirectory, ".env");

    var directory = new DirectoryInfo(AppContext.BaseDirectory);
    while (directory is not null)
    {
        yield return Path.Combine(directory.FullName, ".env");
        directory = directory.Parent;
    }
}
