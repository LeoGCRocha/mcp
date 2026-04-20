using System.ComponentModel;
using ModelContextProtocol.Server;

namespace mcp_shared.Resources;

[McpServerResourceType]
public static class ApiDocumentationResource
{
    [McpServerResource(
        UriTemplate = "docs://users-api-routes",
        Name = "users-api-routes",
        Title = "Users API Routes",
        MimeType = "text/markdown",
        IconSource = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxMjgiIGhlaWdodD0iMTI4IiB2aWV3Qm94PSIwIDAgMTI4IDEyOCI+PHJlY3Qgd2lkdGg9IjEyOCIgIGhlaWdodD0iMTI4IiByeD0iMjAiIGZpbGw9IiMxZjI5Mzc iLz48cGF0aCBkPSJNMzYgMzJoNTZ2MTJIMzZ6bTAgMjZoNTZ2MTJIMzZ6bTAgMjZoMzh2MTJIMzZ6IiBmaWxsPSIjZjhmYWZjIi8+PC9zdmc+")]
    [Description("Documentacao em markdown das rotas da API de usuarios consumida pelo MCP server.")]
    public static string GetUsersApiRoutes()
    {
        var docsPath = Path.Combine(AppContext.BaseDirectory, "docs", "users-api-routes.md");

        if (!File.Exists(docsPath))
        {
            docsPath = Path.Combine(Directory.GetCurrentDirectory(), "docs", "users-api-routes.md");
        }

        if (!File.Exists(docsPath))
        {
            throw new FileNotFoundException("Arquivo de documentacao da API nao encontrado.", docsPath);
        }

        return File.ReadAllText(docsPath);
    }
}
