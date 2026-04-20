using System.ComponentModel;
using ModelContextProtocol.Server;

namespace mcp_shared.Prompt;

[McpServerPromptType]
public static class UsersPrompts
{
    [McpServerPrompt(Name = "users-api-operation-guide", Title = "Users API Operation Guide")]
    [Description("Gera um prompt padrao para orientar o uso das tools do usersApi em uma operacao especifica.")]
    public static string GetUsersApiOperationGuide(
        [Description("Operacao desejada: list_users, get_user, create_user, update_user, delete_user ou add_note")] string operation,
        [Description("Id do usuario quando a operacao exigir")] string? userId = null,
        [Description("Nome do usuario quando a operacao exigir")] string? name = null,
        [Description("Email do usuario quando a operacao exigir")] string? email = null,
        [Description("Conteudo da nota quando a operacao exigir")] string? content = null)
    {
        var normalizedOperation = operation.Trim().ToLowerInvariant();

        return normalizedOperation switch
        {
            "list_users" => """
                Voce esta usando o MCP server `usersApi`.

                Objetivo:
                - listar todos os usuarios cadastrados

                Acao esperada:
                - chamar a tool `get_users`
                - nao enviar argumentos

                Validacao esperada:
                - retornar a lista serializada de usuarios
                - se a lista vier vazia, informar que nenhum usuario foi encontrado
                """,

            "get_user" => $"""
                Voce esta usando o MCP server `usersApi`.

                Objetivo:
                - buscar um usuario especifico pelo id

                Acao esperada:
                - chamar a tool `get_user_by_id`
                - enviar o argumento `userId` com o valor `{userId ?? "<guid-do-usuario>"}`

                Validacao esperada:
                - se o id for invalido, retornar mensagem de id invalido
                - se o usuario nao existir, retornar mensagem de usuario nao encontrado
                - se existir, retornar o usuario serializado
                """,

            "create_user" => $"""
                Voce esta usando o MCP server `usersApi`.

                Objetivo:
                - criar um novo usuario

                Acao esperada:
                - chamar a tool `create_user`
                - enviar os argumentos:
                  - `name`: `{name ?? "<nome>"}`
                  - `email`: `{email ?? "<email>"}`

                Validacao esperada:
                - a tool deve retornar o usuario criado serializado
                - o retorno deve conter `Id`, `Name`, `Email`, `CreatedAtUtc` e `Notes`
                """,

            "update_user" => $"""
                Voce esta usando o MCP server `usersApi`.

                Objetivo:
                - atualizar nome e email de um usuario existente

                Acao esperada:
                - chamar a tool `update_user`
                - enviar os argumentos:
                  - `userId`: `{userId ?? "<guid-do-usuario>"}`
                  - `name`: `{name ?? "<novo-nome>"}`
                  - `email`: `{email ?? "<novo-email>"}`

                Validacao esperada:
                - se o id for invalido, retornar mensagem de id invalido
                - se o usuario nao existir, retornar mensagem de usuario nao encontrado
                - se existir, retornar o usuario atualizado serializado
                """,

            "delete_user" => $"""
                Voce esta usando o MCP server `usersApi`.

                Objetivo:
                - remover um usuario existente

                Acao esperada:
                - chamar a tool `delete_user`
                - enviar o argumento `userId` com o valor `{userId ?? "<guid-do-usuario>"}`

                Validacao esperada:
                - se o id for invalido, retornar mensagem de id invalido
                - se o usuario existir, retornar confirmacao de remocao
                - se o usuario nao existir, retornar mensagem de usuario nao encontrado
                """,

            "add_note" => $"""
                Voce esta usando o MCP server `usersApi`.

                Objetivo:
                - adicionar uma anotacao a um usuario

                Acao esperada:
                - chamar a tool `add_user_note`
                - enviar os argumentos:
                  - `userId`: `{userId ?? "<guid-do-usuario>"}`
                  - `content`: `{content ?? "<conteudo-da-nota>"}`

                Validacao esperada:
                - se o id for invalido, retornar mensagem de id invalido
                - se o usuario nao existir, retornar mensagem de usuario nao encontrado
                - se existir, retornar a nota criada serializada
                """,

            _ => """
                Operacao nao reconhecida.

                Use uma destas opcoes:
                - list_users
                - get_user
                - create_user
                - update_user
                - delete_user
                - add_note
                """
        };
    }
}
