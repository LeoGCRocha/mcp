# Users API Routes

Esta documentação descreve as rotas HTTP da `simple-api` consumidas pelo `mcp-server`.

Objetivo:
- servir como referência rápida para desenvolvimento
- preparar o conteúdo que poderá virar um resource do MCP futuramente
- documentar o contrato entre `UsersApiClient` e a API de usuários

Status:
- documento estático
- ainda não exposto como resource do MCP

## Base URL

A base URL é definida no `mcp-server` pela variável de ambiente `API_BASE_ADDRESS`.

Exemplo:

```env
API_BASE_ADDRESS=http://localhost:5000
```

## Health Route

### `GET /`

Retorna informações básicas da API.

Resposta de exemplo:

```json
{
  "message": "Simple API is running",
  "swagger": "/swagger",
  "endpoints": [
    "GET /users",
    "GET /users/{id}",
    "POST /users",
    "PUT /users/{id}",
    "DELETE /users/{id}",
    "POST /users/{id}/notes"
  ]
}
```

## User Routes

### `GET /users`

Lista todos os usuários.

Tool relacionada:
- `GetUsers`

Resposta de exemplo:

```json
[
  {
    "id": "2e4b1f96-67e8-4f93-8b93-f85e6bc0c681",
    "name": "Maria Silva",
    "email": "maria@email.com",
    "createdAtUtc": "2026-04-18T03:00:00Z",
    "notes": [
      {
        "id": "ef20db7a-47e5-46dc-8781-8cb78c8dc413",
        "content": "Cliente premium",
        "createdAtUtc": "2026-04-18T03:10:00Z"
      }
    ]
  }
]
```

### `GET /users/{id}`

Busca um usuário específico pelo identificador.

Tool relacionada:
- `GetUserById`

Parâmetros:
- `id`: `guid`

Resposta `200 OK`:

```json
{
  "id": "2e4b1f96-67e8-4f93-8b93-f85e6bc0c681",
  "name": "Maria Silva",
  "email": "maria@email.com",
  "createdAtUtc": "2026-04-18T03:00:00Z",
  "notes": []
}
```

Resposta `404 Not Found`:

```json
{
  "message": "User not found."
}
```

### `POST /users`

Cria um novo usuário.

Tool relacionada:
- `CreateUser`

Body:

```json
{
  "name": "Maria Silva",
  "email": "maria@email.com"
}
```

Resposta `201 Created`:

```json
{
  "id": "2e4b1f96-67e8-4f93-8b93-f85e6bc0c681",
  "name": "Maria Silva",
  "email": "maria@email.com",
  "createdAtUtc": "2026-04-18T03:00:00Z",
  "notes": []
}
```

Resposta `400 Bad Request`:

```json
{
  "message": "Name and email are required."
}
```

### `PUT /users/{id}`

Atualiza nome e email de um usuário existente.

Tool relacionada:
- `UpdateUser`

Parâmetros:
- `id`: `guid`

Body:

```json
{
  "name": "Maria Souza",
  "email": "maria.souza@email.com"
}
```

Resposta `200 OK`:

```json
{
  "id": "2e4b1f96-67e8-4f93-8b93-f85e6bc0c681",
  "name": "Maria Souza",
  "email": "maria.souza@email.com",
  "createdAtUtc": "2026-04-18T03:00:00Z",
  "notes": []
}
```

Possíveis erros:
- `400 Bad Request` quando `name` ou `email` vierem vazios
- `404 Not Found` quando o usuário não existir

### `DELETE /users/{id}`

Remove um usuário.

Tool relacionada:
- `DeleteUser`

Parâmetros:
- `id`: `guid`

Resposta `204 No Content`

Possíveis erros:
- `404 Not Found` quando o usuário não existir

### `POST /users/{id}/notes`

Adiciona uma anotação a um usuário existente.

Tool relacionada:
- `AddUserNote`

Parâmetros:
- `id`: `guid`

Body:

```json
{
  "content": "Usuário pediu retorno amanhã."
}
```

Resposta `201 Created`:

```json
{
  "id": "ef20db7a-47e5-46dc-8781-8cb78c8dc413",
  "content": "Usuário pediu retorno amanhã.",
  "createdAtUtc": "2026-04-18T03:10:00Z"
}
```

Possíveis erros:
- `400 Bad Request` quando `content` vier vazio
- `404 Not Found` quando o usuário não existir

## DTOs Principais

### `CreateUserRequest`

```json
{
  "name": "string",
  "email": "string"
}
```

### `UpdateUserRequest`

```json
{
  "name": "string",
  "email": "string"
}
```

### `AddUserNoteRequest`

```json
{
  "content": "string"
}
```

### `UserResponse`

```json
{
  "id": "guid",
  "name": "string",
  "email": "string",
  "createdAtUtc": "datetime",
  "notes": [
    {
      "id": "guid",
      "content": "string",
      "createdAtUtc": "datetime"
    }
  ]
}
```

## Observações

- Os ids usados pela API são `GUID`.
- O `UsersApiClient` usa essas rotas com `HttpClient`.
- As tools do MCP retornam mensagens simples em texto para casos como `id` inválido ou usuário não encontrado.
- Quando esse documento virar resource, ele já pode ser exposto quase sem adaptação estrutural.
