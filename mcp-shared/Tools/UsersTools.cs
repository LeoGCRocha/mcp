using System.ComponentModel;
using System.Text.Json;
using mcp_shared.Dtos.Users;
using ModelContextProtocol.Server;

namespace mcp_shared.Tools;

[McpServerToolType]
public static class UsersTools
{
    [McpServerTool, Description("Busca todos os usuários")]
    public static async Task<string> GetUsers(UsersApiClient client)
    {
        try
        {
            var users = await client.GetUsersAsync();

            return users.Count == 0
                ? "Nenhum usuário encontrado."
                : JsonSerializer.Serialize(users);
        }
        catch (Exception ex)
        {
            return $"Erro ao buscar os usuários: {ex.Message}";
        }
    }

    [McpServerTool, Description("Busca um usuário pelo id")]
    public static async Task<string> GetUserById(
        UsersApiClient client,
        [Description("Id do usuário")] string userId)
    {
        try
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                return "Id de usuário inválido.";
            }

            var user = await client.GetUserByIdAsync(parsedUserId);
            return user is null
                ? "Usuário não encontrado."
                : JsonSerializer.Serialize(user);
        }
        catch (Exception ex)
        {
            return $"Erro ao buscar o usuário: {ex.Message}";
        }
    }

    [McpServerTool, Description("Cria um novo usuário")]
    public static async Task<string> CreateUser(
        UsersApiClient client,
        [Description("Nome do usuário")] string name,
        [Description("Email do usuário")] string email)
    {
        try
        {
            var user = await client.CreateUserAsync(new CreateUserRequest(name, email));
            return JsonSerializer.Serialize(user);
        }
        catch (Exception ex)
        {
            return $"Erro ao criar o usuário: {ex.Message}";
        }
    }

    [McpServerTool, Description("Atualiza um usuário existente")]
    public static async Task<string> UpdateUser(
        UsersApiClient client,
        [Description("Id do usuário")] string userId,
        [Description("Novo nome do usuário")] string name,
        [Description("Novo email do usuário")] string email)
    {
        try
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                return "Id de usuário inválido.";
            }

            var user = await client.UpdateUserAsync(parsedUserId, new UpdateUserRequest(name, email));
            return user is null
                ? "Usuário não encontrado."
                : JsonSerializer.Serialize(user);
        }
        catch (Exception ex)
        {
            return $"Erro ao atualizar o usuário: {ex.Message}";
        }
    }

    [McpServerTool, Description("Remove um usuário")]
    public static async Task<string> DeleteUser(
        UsersApiClient client,
        [Description("Id do usuário")] string userId)
    {
        try
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                return "Id de usuário inválido.";
            }

            var deleted = await client.DeleteUserAsync(parsedUserId);
            return deleted
                ? "Usuário removido com sucesso."
                : "Usuário não encontrado.";
        }
        catch (Exception ex)
        {
            return $"Erro ao remover o usuário: {ex.Message}";
        }
    }

    [McpServerTool, Description("Adiciona uma anotação a um usuário")]
    public static async Task<string> AddUserNote(
        UsersApiClient client,
        [Description("Id do usuário")] string userId,
        [Description("Conteúdo da anotação")] string content)
    {
        try
        {
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                return "Id de usuário inválido.";
            }

            var note = await client.AddNoteAsync(parsedUserId, new AddUserNoteRequest(content));
            return note is null
                ? "Usuário não encontrado."
                : JsonSerializer.Serialize(note);
        }
        catch (Exception ex)
        {
            return $"Erro ao adicionar anotação ao usuário: {ex.Message}";
        }
    }
}
